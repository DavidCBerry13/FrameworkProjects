using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DavidBerry.Framework.Messaging.AzStorageQueues;

/// <summary>
/// Represents an Azure Storage Queue message publisher.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class AzStorageQueueMessagePublisher<T> : IMessagePublisher<T>, IAsyncDisposable, IDisposable where T : class, new()
{

    private readonly QueueClient _queueClient;
    private readonly ILogger<IMessagePublisher<T>> _logger;

    // --------------------------------------------------------------------------------------------------
    // Constructors
    // --------------------------------------------------------------------------------------------------

    internal AzStorageQueueMessagePublisher(string storageAccountName, string queueName, ILogger<IMessagePublisher<T>> logger)
    {
        var credential = new DefaultAzureCredential();
        _queueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"), credential);
        _logger = logger;
    }


    internal AzStorageQueueMessagePublisher(string storageAccountName, string queueName, string storageAccountKey, ILogger<IMessagePublisher<T>> logger)
    {
        var credential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
        _queueClient = new QueueClient(new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"), credential);

        _logger = logger;
    }


    // --------------------------------------------------------------------------------------------------
    // Methods
    // --------------------------------------------------------------------------------------------------

    /// <summary>
    /// Published a message the Azure Storage Queue with the specified message body and properties.
    /// </summary>
    /// <param name="messageBody"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public Task PublishMessageAsync(T messageBody, MessageProperties? properties = null)
    {
        // Create the queue message with the body and properties
        var queueMessage = new QueueMessage<T>
        {
            Body = messageBody,
            Properties = properties ?? new MessageProperties()
        };

        // Serialize the message to JSON
        var json = JsonSerializer.Serialize(queueMessage);

        // Send the message to the queue
        return _queueClient.SendMessageAsync(json);
    }


    // --------------------------------------------------------------------------------------------
    // IDisposable Methods
    // --------------------------------------------------------------------------------------------

    public void Dispose()
    {
        // QueueClient does not require explicit disposal
    }

    public ValueTask DisposeAsync()
    {
        // QueueClient does not require explicit disposal
        return ValueTask.CompletedTask;
    }
}

