using Azure.Core;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using DavidBerry.Framework.Functional;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging.AzStorageQueues;

internal class AzStorageQueueMessageSubscriber<T, K> : IMessageSubscriber<T, K>, IAsyncDisposable, IDisposable where T : class, new() where K : class, new()
{
    private readonly QueueClient _receiveQueueClient;
    private readonly QueueClient _deadLetterQueueClient;
    private readonly TimeSpan _visibilityTimeout;
    private readonly ILogger<IMessagePublisher<T>> _logger;
    private CancellationTokenSource _subscriptionCancellationTokenSource;

    // --------------------------------------------------------------------------------------------------
    // Constructors
    // --------------------------------------------------------------------------------------------------

    internal AzStorageQueueMessageSubscriber(string storageAccountName, string receiveQueueName, string deadLetterQueueName, ILogger<IMessagePublisher<T>> logger,
        int visibilityTimeoutSeconds = 30)
    {
        var credential = new DefaultAzureCredential();
        _receiveQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{receiveQueueName}"), credential);
        _deadLetterQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{deadLetterQueueName}"), credential);
        _visibilityTimeout = TimeSpan.FromMinutes(visibilityTimeoutSeconds);
        _logger = logger;
        _subscriptionCancellationTokenSource = new CancellationTokenSource();
    }


    internal AzStorageQueueMessageSubscriber(string storageAccountName, string receiveQueueName, string deadLetterQueueName, string storageAccountKey, 
        ILogger<IMessagePublisher<T>> logger, int visibilityTimeoutSeconds = 30)
    {
        var credential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
        _receiveQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{receiveQueueName}"), credential);
        _deadLetterQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{deadLetterQueueName}"), credential);
        _visibilityTimeout = TimeSpan.FromMinutes(visibilityTimeoutSeconds);
        _logger = logger;
        _subscriptionCancellationTokenSource = new CancellationTokenSource();
    }

    // --------------------------------------------------------------------------------------------
    // Methods
    // --------------------------------------------------------------------------------------------

    public async Task SubscribeAsync(Func<T, MessageContext, Task<Result<K>>> messagehandler, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting message subscription");

        try
        {
            // Link the provided cancellation token with our internal token source
            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _subscriptionCancellationTokenSource.Token))
            {
                var linkedToken = linkedTokenSource.Token;

                while (!linkedToken.IsCancellationRequested)
                {
                    try
                    {
                        // Receive messages from the queue
                        var messages = await _receiveQueueClient.ReceiveMessagesAsync(maxMessages: 1, visibilityTimeout: _visibilityTimeout, cancellationToken: linkedToken);

                    if (messages.Value.Count() == 0)
                    {
                        // No messages available, wait a bit before trying again
                        await Task.Delay(1000, linkedToken);
                        continue;
                    }

                    foreach (var message in messages.Value)
                    {
                        try
                        {
                            _logger.LogDebug("Processing message with ID: {MessageId}", message.MessageId);

                            // Deserialize the QueueMessage from JSON
                            QueueMessage<T>? queueMessage = null;
                            T? messageObject = null;
                            try
                            {
                                queueMessage = JsonSerializer.Deserialize<QueueMessage<T>>(message.Body.ToString());
                                if (queueMessage == null)
                                {
                                    throw new JsonException("Deserialized message is null");
                                }
                                messageObject = queueMessage.Body;
                            }
                            catch (JsonException ex)
                            {
                                _logger.LogError(ex, "Failed to deserialize message with ID: {MessageId}. Moving to dead letter queue.", message.MessageId);
                                await MoveToDeadLetterQueueAsync(message, "DeserializationError", $"Failed to deserialize JSON: {ex.Message}");
                                await _receiveQueueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                                continue;
                            }

                            // Create message context from the queue message
                            var messageContext = new MessageContext
                            {
                                MessageId = message.MessageId,
                                SequenceNumber = Convert.ToInt64(message.MessageId.GetHashCode()), // Queue doesn't have sequence numbers, using hash
                                DeliveryCount = (int)message.DequeueCount,
                                EnqueueTime = message.InsertedOn ?? DateTimeOffset.UtcNow
                            };

                            // Invoke the message handler
                            Result<K>? result = null;
                            try
                            {
                                result = await messagehandler(messageObject, messageContext);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Exception occurred while processing message with ID: {MessageId}. Moving to dead letter queue.", message.MessageId);
                                await MoveToDeadLetterQueueAsync(message, "HandlerException", $"Exception: {ex.GetType().Name} - {ex.Message}");
                                await _receiveQueueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                                continue;
                            }

                            // Check the result
                            if (result?.IsSuccess == true)
                            {
                                _logger.LogDebug("Message with ID: {MessageId} processed successfully. Deleting message.", message.MessageId);
                                await _receiveQueueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                            }
                            else
                            {
                                _logger.LogWarning("Message handler returned failure for message ID: {MessageId}. Moving to dead letter queue. Error: {Error}",
                                    message.MessageId, result?.Error?.Message ?? "Unknown error");
                                await MoveToDeadLetterQueueAsync(message, "HandlerFailure", result?.Error?.Message ?? "Handler returned failure");
                                await _receiveQueueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Unexpected error processing message with ID: {MessageId}", message?.MessageId ?? "Unknown");
                        }
                    }
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Message subscription cancelled");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error receiving messages from queue");
                        await Task.Delay(1000, linkedToken);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal error in message subscription");
            throw;
        }

        _logger.LogInformation("Message subscription stopped");
    }

    private async Task MoveToDeadLetterQueueAsync(QueueMessage message, string deadLetterReason, string deadLetterErrorDescription)
    {
        try
        {
            // Create an error message with the raw message body preserved
            var errorMessage = new QueueErrorMessage
            {
                SequenceNumber = Convert.ToInt64(message.MessageId.GetHashCode()),
                EnqueueTime = message.InsertedOn ?? DateTimeOffset.UtcNow,
                DeadLetterReason = deadLetterReason,
                DeadLetterErrorDescription = deadLetterErrorDescription,
                MessageBody = message.Body.ToString()  // Always preserve the raw message
            };

            await _deadLetterQueueClient.SendMessageAsync(JsonSerializer.Serialize(errorMessage));
            _logger.LogInformation("Message {MessageId} moved to dead letter queue. Reason: {Reason}", message.MessageId, deadLetterReason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to move message {MessageId} to dead letter queue", message.MessageId);
        }
    }


    public Task StopAsync()
    {
        _logger.LogInformation("Stopping message subscription");
        _subscriptionCancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }


    // --------------------------------------------------------------------------------------------
    // IDisposable Methods
    // --------------------------------------------------------------------------------------------

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing AzStorageQueueSubscriber");

        // Cancel any ongoing subscription
        try
        {
            _subscriptionCancellationTokenSource?.Cancel();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error cancelling subscription during disposal");
        }

        // Dispose of the cancellation token source
        _subscriptionCancellationTokenSource?.Dispose();

        GC.SuppressFinalize(this);
    }
}

