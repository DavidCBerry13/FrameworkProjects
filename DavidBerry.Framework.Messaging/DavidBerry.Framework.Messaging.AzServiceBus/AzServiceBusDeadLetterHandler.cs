using Azure;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using DavidBerry.Framework.Functional;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging.AzServiceBus;

/// <summary>
/// Handles dead letter messages for Azure Service Bus topics/subscriptions and queues.
/// </summary>
/// <typeparam name="T">The message type</typeparam>
internal class AzServiceBusDeadLetterHandler<T> : IDeadLetterHandler<T>, IAsyncDisposable where T : class, new()
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusAdministrationClient _adminClient;
    private readonly ServiceBusReceiver _receiver;
    private readonly ServiceBusSender _sender;
    private readonly string _topicOrQueueName;
    private readonly string? _subscriptionName;
    private readonly ILogger<IDeadLetterHandler<T>> _logger;
    private bool _disposed;

    // --------------------------------------------------------------------------------------------
    // Constructors - Topic/Subscription
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusDeadLetterHandler{T}"/> class for a topic subscription.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name</param>
    /// <param name="subscriptionName">The subscription name</param>
    /// <param name="logger">The logger for the handler</param>
    internal AzServiceBusDeadLetterHandler(string serviceBusNamespace, string topicName, string subscriptionName,
        ILogger<IDeadLetterHandler<T>> logger)
    {
        var credential = new DefaultAzureCredential();
        var fullyQualifiedNamespace = $"{serviceBusNamespace}.servicebus.windows.net";

        _client = new ServiceBusClient(fullyQualifiedNamespace, credential);
        _adminClient = new ServiceBusAdministrationClient(fullyQualifiedNamespace, credential);
        _receiver = _client.CreateReceiver(topicName, subscriptionName, 
            new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock, SubQueue = SubQueue.DeadLetter });
        _sender = _client.CreateSender(topicName);
        _topicOrQueueName = topicName;
        _subscriptionName = subscriptionName;
        _logger = logger;
        _disposed = false;
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusDeadLetterHandler{T}"/> class for a topic subscription.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name</param>
    /// <param name="subscriptionName">The subscription name</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="logger">The logger for the handler</param>
    internal AzServiceBusDeadLetterHandler(string serviceBusNamespace, string topicName, string subscriptionName, AzureNamedKeyCredential credential,
        ILogger<IDeadLetterHandler<T>> logger)
    {
        var fullyQualifiedNamespace = $"{serviceBusNamespace}.servicebus.windows.net";

        _client = new ServiceBusClient(fullyQualifiedNamespace, credential);
        _adminClient = new ServiceBusAdministrationClient(fullyQualifiedNamespace, credential);
        _receiver = _client.CreateReceiver(topicName, subscriptionName,
            new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock, SubQueue = SubQueue.DeadLetter });
        _sender = _client.CreateSender(topicName);
        _topicOrQueueName = topicName;
        _subscriptionName = subscriptionName;
        _logger = logger;
        _disposed = false;
    }


    // --------------------------------------------------------------------------------------------
    // Constructors - Queue
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusDeadLetterHandler{T}"/> class for a queue.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name</param>
    /// <param name="logger">The logger for the handler</param>
    internal AzServiceBusDeadLetterHandler(string serviceBusNamespace, string queueName,
        ILogger<IDeadLetterHandler<T>> logger)
    {
        var credential = new DefaultAzureCredential();
        var fullyQualifiedNamespace = $"{serviceBusNamespace}.servicebus.windows.net";

        _client = new ServiceBusClient(fullyQualifiedNamespace, credential);
        _adminClient = new ServiceBusAdministrationClient(fullyQualifiedNamespace, credential);
        _receiver = _client.CreateReceiver(queueName,
            new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock, SubQueue = SubQueue.DeadLetter });
        _sender = _client.CreateSender(queueName);
        _topicOrQueueName = queueName;
        _subscriptionName = null;
        _logger = logger;
        _disposed = false;
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusDeadLetterHandler{T}"/> class for a queue.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name</param>
    /// <param name="logger">The logger for the handler</param>
    internal AzServiceBusDeadLetterHandler(string serviceBusNamespace, string queueName, AzureNamedKeyCredential credential,
        ILogger<IDeadLetterHandler<T>> logger)
    {
        var fullyQualifiedNamespace = $"{serviceBusNamespace}.servicebus.windows.net";

        _client = new ServiceBusClient(fullyQualifiedNamespace, credential);
        _adminClient = new ServiceBusAdministrationClient(fullyQualifiedNamespace, credential);
        _receiver = _client.CreateReceiver(queueName,
            new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock, SubQueue = SubQueue.DeadLetter });
        _sender = _client.CreateSender(queueName);
        _topicOrQueueName = queueName;
        _subscriptionName = null;
        _logger = logger;
        _disposed = false;
    }

    // --------------------------------------------------------------------------------------------
    // Methods
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Gets the count of messages in the dead letter queue.
    /// </summary>
    public async Task<Result<long>> GetMessageCountAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Getting dead letter message count for {Topic}{Subscription}",
                _topicOrQueueName, _subscriptionName != null ? $"/{_subscriptionName}" : "");

            long count;
            if (_subscriptionName != null)
            {
                var properties = await _adminClient.GetSubscriptionRuntimePropertiesAsync(_topicOrQueueName, _subscriptionName, cancellationToken);
                count = properties.Value.DeadLetterMessageCount;
            }
            else
            {
                var properties = await _adminClient.GetQueueRuntimePropertiesAsync(_topicOrQueueName, cancellationToken);
                count = properties.Value.DeadLetterMessageCount;
            }

            _logger.LogInformation("Dead letter message count: {Count}", count);
            return Result.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dead letter message count");
            return Result.Failure<long>(new Error(ex.Message));
        }
    }

    /// <summary>
    /// Peeks at messages in the dead letter queue without removing them.
    /// </summary>
    public async Task<Result<IReadOnlyList<DeadLetterMessage<T>>>> PeekMessagesAsync(int maxMessages, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Peeking at {MaxMessages} dead letter messages", maxMessages);

            var messages = await _receiver.PeekMessagesAsync(maxMessages, cancellationToken: cancellationToken);
            var deadLetterMessages = new List<DeadLetterMessage<T>>();

            foreach (var message in messages)
            {
                try
                {
                    var body = JsonSerializer.Deserialize<T>(message.Body.ToString());
                    deadLetterMessages.Add(new DeadLetterMessage<T>
                    {
                        SequenceNumber = message.SequenceNumber,
                        EnqueueTime = message.EnqueuedTime,
                        DeadLetterReason = message.DeadLetterReason,
                        DeadLetterErrorDescription = message.DeadLetterErrorDescription,
                        Body = body
                    });
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to deserialize dead letter message sequence number {SequenceNumber}", message.SequenceNumber);
                    deadLetterMessages.Add(new DeadLetterMessage<T>
                    {
                        SequenceNumber = message.SequenceNumber,
                        EnqueueTime = message.EnqueuedTime,
                        DeadLetterReason = message.DeadLetterReason,
                        DeadLetterErrorDescription = message.DeadLetterErrorDescription,
                        Body = null
                    });
                }
            }

            _logger.LogInformation("Peeked {Count} dead letter messages", deadLetterMessages.Count);
            return Result.Success<IReadOnlyList<DeadLetterMessage<T>>>(deadLetterMessages.AsReadOnly());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error peeking dead letter messages");
            return Result.Failure<IReadOnlyList<DeadLetterMessage<T>>>(new Error(ex.Message));
        }
    }

    /// <summary>
    /// Resubmits a dead letter message back to the original topic/queue.
    /// </summary>
    public async Task<Result> ResubmitMessageAsync(long sequenceNumber, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Resubmitting dead letter message with sequence number {SequenceNumber}", sequenceNumber);

            var messages = await _receiver.ReceiveMessagesAsync(maxMessages: 100, maxWaitTime: TimeSpan.FromSeconds(5), cancellationToken: cancellationToken);
            var messageToResubmit = messages.FirstOrDefault(m => m.SequenceNumber == sequenceNumber);

            if (messageToResubmit == null)
            {
                _logger.LogWarning("Dead letter message with sequence number {SequenceNumber} not found", sequenceNumber);
                return Result.Failure(new Error($"Message with sequence number {sequenceNumber} not found"));
            }

            try
            {
                // Create a new message with the body from the dead letter message
                var newMessage = new ServiceBusMessage(messageToResubmit.Body);
                newMessage.ContentType = messageToResubmit.ContentType;

                // Copy application properties
                if (messageToResubmit.ApplicationProperties != null)
                {
                    foreach (var kvp in messageToResubmit.ApplicationProperties)
                    {
                        newMessage.ApplicationProperties[kvp.Key] = kvp.Value;
                    }
                }

                // Send to the original topic/queue
                await _sender.SendMessageAsync(newMessage, cancellationToken);
                _logger.LogInformation("Successfully resubmitted dead letter message with sequence number {SequenceNumber}", sequenceNumber);

                // Complete the message to remove it from the dead letter queue
                await _receiver.CompleteMessageAsync(messageToResubmit, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resubmitting message, attempting to abandon");
                await _receiver.AbandonMessageAsync(messageToResubmit, cancellationToken: cancellationToken);
                return Result.Failure(new Error($"Error resubmitting message: {ex.Message}"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ResubmitMessageAsync");
            return Result.Failure(new Error(ex.Message));
        }
    }

    /// <summary>
    /// Resubmits all dead letter messages back to the original topic/queue.
    /// </summary>
    public async Task<Result> ResubmitAllMessagesAsync(int maxMessages, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Resubmitting up to {MaxMessages} dead letter messages", maxMessages);

            var messages = await _receiver.ReceiveMessagesAsync(maxMessages, cancellationToken: cancellationToken);
            int successCount = 0;
            int failureCount = 0;

            foreach (var message in messages)
            {
                try
                {
                    // Create a new message with the body from the dead letter message
                    var newMessage = new ServiceBusMessage(message.Body);
                    newMessage.ContentType = message.ContentType;

                    // Copy application properties
                    if (message.ApplicationProperties != null)
                    {
                        foreach (var kvp in message.ApplicationProperties)
                        {
                            newMessage.ApplicationProperties[kvp.Key] = kvp.Value;
                        }
                    }

                    // Send to the original topic/queue
                    await _sender.SendMessageAsync(newMessage, cancellationToken);
                    await _receiver.CompleteMessageAsync(message, cancellationToken);
                    successCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to resubmit message with sequence number {SequenceNumber}", message.SequenceNumber);
                    try
                    {
                        await _receiver.AbandonMessageAsync(message, cancellationToken: cancellationToken);
                    }
                    catch (Exception abandonEx)
                    {
                        _logger.LogError(abandonEx, "Failed to abandon message with sequence number {SequenceNumber}", message.SequenceNumber);
                    }
                    failureCount++;
                }
            }

            _logger.LogInformation("Resubmitted {SuccessCount} messages with {FailureCount} failures", successCount, failureCount);

            if (failureCount > 0)
            {
                return Result.Failure(new Error($"Resubmitted {successCount} messages with {failureCount} failures"));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ResubmitAllMessagesAsync");
            return Result.Failure(new Error(ex.Message));
        }
    }

    /// <summary>
    /// Deletes a dead letter message.
    /// </summary>
    public async Task<Result> DeleteMessageAsync(long sequenceNumber, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Deleting dead letter message with sequence number {SequenceNumber}", sequenceNumber);

            var messages = await _receiver.ReceiveMessagesAsync(maxMessages: 100, maxWaitTime: TimeSpan.FromSeconds(5), cancellationToken: cancellationToken);
            var messageToDelete = messages.FirstOrDefault(m => m.SequenceNumber == sequenceNumber);

            if (messageToDelete == null)
            {
                _logger.LogWarning("Dead letter message with sequence number {SequenceNumber} not found", sequenceNumber);
                return Result.Failure(new Error($"Message with sequence number {sequenceNumber} not found"));
            }

            try
            {
                await _receiver.CompleteMessageAsync(messageToDelete, cancellationToken);
                _logger.LogInformation("Successfully deleted dead letter message with sequence number {SequenceNumber}", sequenceNumber);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message, attempting to abandon");
                await _receiver.AbandonMessageAsync(messageToDelete, cancellationToken: cancellationToken);
                return Result.Failure(new Error($"Error deleting message: {ex.Message}"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteMessageAsync");
            return Result.Failure(new Error(ex.Message));
        }
    }

    /// <summary>
    /// Deletes all dead letter messages up to the specified limit.
    /// </summary>
    public async Task<Result> DeleteAllMessagesAsync(long sequenceNumber, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting dead letter messages");

            var messages = await _receiver.ReceiveMessagesAsync(maxMessages: 100, cancellationToken: cancellationToken);
            int deleteCount = 0;
            int failureCount = 0;

            foreach (var message in messages)
            {
                try
                {
                    await _receiver.CompleteMessageAsync(message, cancellationToken);
                    deleteCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete message with sequence number {SequenceNumber}", message.SequenceNumber);
                    try
                    {
                        await _receiver.AbandonMessageAsync(message, cancellationToken: cancellationToken);
                    }
                    catch (Exception abandonEx)
                    {
                        _logger.LogError(abandonEx, "Failed to abandon message with sequence number {SequenceNumber}", message.SequenceNumber);
                    }
                    failureCount++;
                }
            }

            _logger.LogInformation("Deleted {DeleteCount} messages with {FailureCount} failures", deleteCount, failureCount);

            if (failureCount > 0)
            {
                return Result.Failure(new Error($"Deleted {deleteCount} messages with {failureCount} failures"));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteAllMessagesAsync");
            return Result.Failure(new Error(ex.Message));
        }
    }

    // --------------------------------------------------------------------------------------------
    // IAsyncDisposable Methods
    // --------------------------------------------------------------------------------------------

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        _logger.LogInformation("Disposing AzServiceBusDeadLetterHandler");

        try
        {
            await _receiver.DisposeAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error disposing receiver");
        }

        try
        {
            await _sender.DisposeAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error disposing sender");
        }

        try
        {
            await _client.DisposeAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error disposing client");
        }

        GC.SuppressFinalize(this);
    }
}
