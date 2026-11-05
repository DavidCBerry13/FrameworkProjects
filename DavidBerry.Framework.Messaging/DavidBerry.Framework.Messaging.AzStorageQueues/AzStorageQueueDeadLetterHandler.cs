using Azure.Core;
using Azure.Identity;
using Azure.Storage.Queues;
using DavidBerry.Framework.Functional;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging.AzStorageQueues;

internal class AzStorageQueueDeadLetterHandler<T> : IDeadLetterHandler<T>, IAsyncDisposable where T : class, new()
{
    private readonly QueueClient _deadLetterQueueClient;

    private readonly QueueClient _receiveQueueClient;

    private readonly ILogger<IDeadLetterHandler<T>> _logger;

    // --------------------------------------------------------------------------------------------------
    // Constructors
    // --------------------------------------------------------------------------------------------------
    internal AzStorageQueueDeadLetterHandler(string storageAccountName, string deadLetterQueueName, string receiveQueueName, ILogger<IDeadLetterHandler<T>> logger)
    {
        var credential = new DefaultAzureCredential();
        _deadLetterQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{deadLetterQueueName}"), credential);
        _receiveQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{receiveQueueName}"), credential);
        _logger = logger;
    }


    internal AzStorageQueueDeadLetterHandler(string storageAccountName, string deadLetterQueueName, string receiveQueueName, TokenCredential credential, ILogger<IDeadLetterHandler<T>> logger)
    {
        _deadLetterQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{deadLetterQueueName}"), credential);
        _receiveQueueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{receiveQueueName}"), credential);
        _logger = logger;
    }

    // --------------------------------------------------------------------------------------------------
    // Methods
    // --------------------------------------------------------------------------------------------------

    public async Task<Result> DeleteAllMessagesAsync(long sequenceNumber, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting dead letter messages");

            var messages = await _deadLetterQueueClient.ReceiveMessagesAsync(maxMessages: 32, cancellationToken: cancellationToken);
            int deleteCount = 0;
            int failureCount = 0;

            foreach (var message in messages.Value)
            {
                try
                {
                    await _deadLetterQueueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);
                    deleteCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete message with ID {MessageId}", message.MessageId);
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

    public async Task<Result> DeleteMessageAsync(long sequenceNumber, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Deleting dead letter message with sequence number {SequenceNumber}", sequenceNumber);

            var messages = await _deadLetterQueueClient.ReceiveMessagesAsync(maxMessages: 32, cancellationToken: cancellationToken);
            var messageToDelete = messages.Value.FirstOrDefault(m => Convert.ToInt64(m.MessageId.GetHashCode()) == sequenceNumber);

            if (messageToDelete == null)
            {
                _logger.LogWarning("Dead letter message with sequence number {SequenceNumber} not found", sequenceNumber);
                return Result.Failure(new Error($"Message with sequence number {sequenceNumber} not found"));
            }

            try
            {
                await _deadLetterQueueClient.DeleteMessageAsync(messageToDelete.MessageId, messageToDelete.PopReceipt, cancellationToken);
                _logger.LogInformation("Successfully deleted dead letter message with sequence number {SequenceNumber}", sequenceNumber);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message");
                return Result.Failure(new Error($"Error deleting message: {ex.Message}"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteMessageAsync");
            return Result.Failure(new Error(ex.Message));
        }
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing AzStorageQueueDeadLetterHandler");
        // QueueClient does not require explicit disposal
        await ValueTask.CompletedTask;
    }

    public async Task<Result<long>> GetMessageCountAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Getting dead letter message count");

            var properties = await _deadLetterQueueClient.GetPropertiesAsync(cancellationToken);
            long count = properties.Value.ApproximateMessagesCount;

            _logger.LogInformation("Dead letter message count: {Count}", count);
            return Result.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dead letter message count");
            return Result.Failure<long>(new Error(ex.Message));
        }
    }

    public async Task<Result<IReadOnlyList<DeadLetterMessage<T>>>> PeekMessagesAsync(int maxMessages, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Peeking at {MaxMessages} dead letter messages", maxMessages);

            var messages = await _deadLetterQueueClient.PeekMessagesAsync(maxMessages: maxMessages, cancellationToken: cancellationToken);
            var deadLetterMessages = new List<DeadLetterMessage<T>>();

            foreach (var message in messages.Value)
            {
                try
                {
                    // Deserialize as QueueErrorMessage (the write-side envelope)
                    var errorMessage = JsonSerializer.Deserialize<QueueErrorMessage>(message.Body.ToString());

                    if (errorMessage == null)
                    {
                        _logger.LogWarning("Failed to deserialize error message {MessageId}: result was null", message.MessageId);
                        continue;
                    }

                    // Create the read-side DeadLetterMessage<T>
                    var deadLetterMessage = new DeadLetterMessage<T>
                    {
                        SequenceNumber = errorMessage.SequenceNumber,
                        EnqueueTime = errorMessage.EnqueueTime,
                        DeadLetterReason = errorMessage.DeadLetterReason,
                        DeadLetterErrorDescription = errorMessage.DeadLetterErrorDescription,
                        RawMessageBody = errorMessage.MessageBody
                    };

                    // Try to deserialize the raw message body as T
                    try
                    {
                        deadLetterMessage.Body = JsonSerializer.Deserialize<T>(errorMessage.MessageBody);
                    }
                    catch
                    {
                        // Body remains null if deserialization fails; RawMessageBody preserves the original
                        _logger.LogDebug("Could not deserialize raw message body as {TypeName} for sequence {SequenceNumber}", typeof(T).Name, errorMessage.SequenceNumber);
                    }

                    deadLetterMessages.Add(deadLetterMessage);
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to deserialize dead letter message {MessageId}", message.MessageId);
                    deadLetterMessages.Add(new DeadLetterMessage<T>
                    {
                        SequenceNumber = Convert.ToInt64(message.MessageId.GetHashCode()),
                        EnqueueTime = message.InsertedOn ?? DateTimeOffset.UtcNow,
                        DeadLetterReason = "DeserializationError",
                        DeadLetterErrorDescription = ex.Message,
                        Body = null,
                        RawMessageBody = message.Body.ToString()
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

    public async Task<Result> ResubmitAllMessagesAsync(int maxMessages, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Resubmitting up to {MaxMessages} dead letter messages", maxMessages);

            var messages = await _deadLetterQueueClient.ReceiveMessagesAsync(maxMessages: maxMessages, cancellationToken: cancellationToken);
            int successCount = 0;
            int failureCount = 0;

            foreach (var message in messages.Value)
            {
                try
                {
                    // Deserialize the error message to extract the raw original body
                    var errorMessage = JsonSerializer.Deserialize<QueueErrorMessage>(message.Body.ToString());

                    if (errorMessage == null)
                    {
                        _logger.LogWarning("Failed to deserialize error message {MessageId} for resubmission", message.MessageId);
                        failureCount++;
                        continue;
                    }

                    // Send the raw original message body back to the receive queue
                    await _receiveQueueClient.SendMessageAsync(errorMessage.MessageBody, cancellationToken);

                    // Delete from dead letter queue
                    await _deadLetterQueueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);
                    successCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to resubmit message with ID {MessageId}", message.MessageId);
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

    public async Task<Result> ResubmitMessageAsync(long sequenceNumber, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Resubmitting dead letter message with sequence number {SequenceNumber}", sequenceNumber);

            var messages = await _deadLetterQueueClient.ReceiveMessagesAsync(maxMessages: 32, cancellationToken: cancellationToken);
            var messageToResubmit = messages.Value.FirstOrDefault(m => Convert.ToInt64(m.MessageId.GetHashCode()) == sequenceNumber);

            if (messageToResubmit == null)
            {
                _logger.LogWarning("Dead letter message with sequence number {SequenceNumber} not found", sequenceNumber);
                return Result.Failure(new Error($"Message with sequence number {sequenceNumber} not found"));
            }

            try
            {
                // Deserialize the error message to extract the raw original body
                var errorMessage = JsonSerializer.Deserialize<QueueErrorMessage>(messageToResubmit.Body.ToString());

                if (errorMessage == null)
                {
                    _logger.LogError("Failed to deserialize error message {MessageId} for resubmission", messageToResubmit.MessageId);
                    return Result.Failure(new Error($"Failed to deserialize error message"));
                }

                // Send the raw original message body back to the receive queue
                await _receiveQueueClient.SendMessageAsync(errorMessage.MessageBody, cancellationToken);

                _logger.LogInformation("Successfully resubmitted dead letter message with sequence number {SequenceNumber}", sequenceNumber);

                // Delete from dead letter queue
                await _deadLetterQueueClient.DeleteMessageAsync(messageToResubmit.MessageId, messageToResubmit.PopReceipt, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resubmitting message");
                return Result.Failure(new Error($"Error resubmitting message: {ex.Message}"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ResubmitMessageAsync");
            return Result.Failure(new Error(ex.Message));
        }
    }
}

