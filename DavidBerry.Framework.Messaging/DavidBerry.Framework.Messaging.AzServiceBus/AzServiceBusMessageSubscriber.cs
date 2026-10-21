using Azure.Messaging.ServiceBus;
using Azure.Identity;
using DavidBerry.Framework.Functional;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;

namespace DavidBerry.Framework.Messaging.AzServiceBus;

/// <summary>
/// Represents a message subscriber that subscribes to messages from an Azure Service Bus topic or queue.
/// </summary>
/// <typeparam name="T">The type of message to subscribe to</typeparam>
/// <typeparam name="K">The type that will be returned by the message handler function</typeparam>
public class AzServiceBusMessageSubscriber<T, K> : IMessageSubscriber<T, K>, IDisposable, IAsyncDisposable where T : class, new() where K : class, new()
{


    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<IMessageSubscriber<T, K>> _logger;
    private bool _disposed;

    // --------------------------------------------------------------------------------------------
    // Constructors - Topic/Subscription
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusMessageSubscriber{T, K}"/> class for Service Bus topics and subscriptions.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the Service Bus</param>
    /// <param name="logger">The logger for the subscriber</param>
    /// <param name="autoCompleteMessages">Indicates whether messages should be automatically completed</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent calls. Default is 1</param>
    /// <param name="prefetchCount">Number of messages to prefetch. Default is 0</param>
    /// <param name="receiveMode">The receive mode to use. Default is PeekLock</param>
    internal AzServiceBusMessageSubscriber(string serviceBusNamespace, string topicName, string subscriptionName,
        ILogger<IMessageSubscriber<T, K>> logger,
        bool autoCompleteMessages = true, int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
    {       
        var credential = new DefaultAzureCredential();
        _client = new ServiceBusClient($"{serviceBusNamespace}.servicebus.windows.net", credential);

        var processorOptions = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = autoCompleteMessages,
            MaxConcurrentCalls = maxConcurrentCalls,
            PrefetchCount = prefetchCount,
            ReceiveMode = receiveMode
        };

        _processor = _client.CreateProcessor(topicName, subscriptionName, processorOptions);
        _logger = logger;
        _disposed = false;
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusMessageSubscriber{T, K}"/> class for Service Bus topics and subscriptions.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the Service Bus</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="logger">The logger for the subscriber</param>
    /// <param name="autoCompleteMessages">Indicates whether messages should be automatically completed</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent calls. Default is 1</param>
    /// <param name="prefetchCount">Number of messages to prefetch. Default is 0</param>
    /// <param name="receiveMode">The receive mode to use. Default is PeekLock</param>
    internal AzServiceBusMessageSubscriber(string serviceBusNamespace, string topicName, string subscriptionName, AzureNamedKeyCredential credential,
        ILogger<IMessageSubscriber<T, K>> logger,
        bool autoCompleteMessages = true, int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
    {
        _client = new ServiceBusClient($"{serviceBusNamespace}.servicebus.windows.net", credential);

        var processorOptions = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = autoCompleteMessages,
            MaxConcurrentCalls = maxConcurrentCalls,
            PrefetchCount = prefetchCount,
            ReceiveMode = receiveMode
        };

        _processor = _client.CreateProcessor(topicName, subscriptionName, processorOptions);
        _logger = logger;
        _disposed = false;
    }

    // --------------------------------------------------------------------------------------------
    // Constructors - Queue
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusMessageSubscriber{T, K}"/> class for Service Bus queues.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="logger">The logger for the subscriber</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent calls. Default is 1</param>
    /// <param name="prefetchCount">Number of messages to prefetch. Default is 0</param>
    /// <param name="receiveMode">The receive mode to use. Default is PeekLock</param>
    internal AzServiceBusMessageSubscriber(string serviceBusNamespace, string queueName,
        ILogger<IMessageSubscriber<T, K>> logger,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
    {
        var credential = new DefaultAzureCredential();
        _client = new ServiceBusClient($"{serviceBusNamespace}.servicebus.windows.net", credential);

        var processorOptions = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = maxConcurrentCalls,
            PrefetchCount = prefetchCount,
            ReceiveMode = receiveMode
        };

        _processor = _client.CreateProcessor(queueName, processorOptions);
        _logger = logger;
        _disposed = false;
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusMessageSubscriber{T, K}"/> class for Service Bus queues.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="logger">The logger for the subscriber</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent calls. Default is 1</param>
    /// <param name="prefetchCount">Number of messages to prefetch. Default is 0</param>
    /// <param name="receiveMode">The receive mode to use. Default is PeekLock</param>
    internal AzServiceBusMessageSubscriber(string serviceBusNamespace, string queueName, AzureNamedKeyCredential credential,
        ILogger<IMessageSubscriber<T, K>> logger,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
    {
        _client = new ServiceBusClient($"{serviceBusNamespace}.servicebus.windows.net", credential);

        var processorOptions = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = maxConcurrentCalls,
            PrefetchCount = prefetchCount,
            ReceiveMode = receiveMode
        };

        _processor = _client.CreateProcessor(queueName, processorOptions);
        _logger = logger;
        _disposed = false;
    }


    // --------------------------------------------------------------------------------------------
    // Methods
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Subscribes to messages from the Service Bus and processes them using the provided message handler.
    /// </summary>
    /// <param name="messagehandler">The function to handle incoming messages</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public async Task SubscribeAsync(Func<T, MessageContext, Task<Result<K>>> messagehandler, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting message subscription");

        _processor.ProcessMessageAsync += async args =>
        {
            try
            {
                _logger.LogDebug("Processing message with ID: {MessageId}", args.Message.MessageId);

                // Deserialize the message body from JSON
                T? messageObject = null;
                try
                {
                    messageObject = JsonSerializer.Deserialize<T>(args.Message.Body.ToString());
                    if (messageObject == null)
                    {
                        throw new JsonException("Deserialized message is null");
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize message with ID: {MessageId}. Moving to dead letter queue.", args.Message.MessageId);
                    await args.DeadLetterMessageAsync(args.Message, deadLetterReason: "DeserializationError",
                        deadLetterErrorDescription: $"Failed to deserialize JSON: {ex.Message}");
                    return;
                }

                // Create message context from the service bus message
                var messageContext = new MessageContext
                {
                    MessageId = args.Message.MessageId,
                    SequenceNumber = args.Message.SequenceNumber,
                    DeliveryCount = args.Message.DeliveryCount,
                    EnqueueTime = args.Message.EnqueuedTime
                };

                // Invoke the message handler
                Result<K>? result = null;
                try
                {
                    result = await messagehandler(messageObject, messageContext);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while processing message with ID: {MessageId}. Moving to dead letter queue.", args.Message.MessageId);
                    await args.DeadLetterMessageAsync(args.Message, deadLetterReason: "HandlerException",
                        deadLetterErrorDescription: $"Exception: {ex.GetType().Name} - {ex.Message}");
                    return;
                }

                // Check the result
                if (result?.IsSuccess == true)
                {
                    _logger.LogDebug("Message with ID: {MessageId} processed successfully. Completing message.", args.Message.MessageId);
                    await args.CompleteMessageAsync(args.Message);
                }
                else
                {
                    _logger.LogWarning("Message handler returned failure for message ID: {MessageId}. Moving to dead letter queue. Error: {Error}",
                        args.Message.MessageId, result?.Error?.Message ?? "Unknown error");
                    await args.DeadLetterMessageAsync(args.Message, deadLetterReason: "HandlerFailure",
                        deadLetterErrorDescription: result?.Error?.Message ?? "Handler returned failure");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing message with ID: {MessageId}", args.Message?.MessageId ?? "Unknown");
            }
        };

        _processor.ProcessErrorAsync += args =>
        {
            _logger.LogError(args.Exception, "Error processing message. Error source: {ErrorSource}", args.ErrorSource);
            return Task.CompletedTask;
        };

        await _processor.StartProcessingAsync(cancellationToken);
        _logger.LogInformation("Message subscription started");
    }


    public async Task StopAsync()
    {
        _logger.LogInformation("Stopping message subscription");
        await _processor.StopProcessingAsync();
        _logger.LogInformation("Message subscription stopped");
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
        if (_disposed)
            return;

        _disposed = true;

        _logger.LogInformation("Disposing AzServiceBusMessageSubscriber");

        try
        {
            await _processor.StopProcessingAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error stopping processor during disposal");
        }

        await _processor.DisposeAsync();
        await _client.DisposeAsync();

        GC.SuppressFinalize(this);
    }

}

