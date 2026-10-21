using Azure;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using DavidBerry.Framework.Functional;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging.AzServiceBus;

/// <summary>
/// Represents a message publisher that publishes messages to an Azure Service Bus topic.
/// </summary>
/// <typeparam name="T">The type of message to publish</typeparam>
public class AzServiceBusMessagePublisher<T> : IMessagePublisher<T>, IAsyncDisposable, IDisposable where T : class, new()
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusSender _serviceBusSender;
    private readonly ILogger<IMessagePublisher<T>> _logger;
    private bool _disposed;

    // --------------------------------------------------------------------------------------------
    // Constructors
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusMessagePublisher{T}"/> class.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueOrTopicName">The queue or topic name on the Service Bus</param>
    /// <param name="logger">The logger for the publisher</param>
    /// <param name="retryMode">The retry mode to use.  Default is Exponential</param>
    /// <param name="maxRetries">Maximum number of retries.  Default is 3</param>
    /// <param name="delaySeconds">Initial delay in seconds. Default is 1</param>
    /// <param name="maxDelaySeconds">Maximum delay in seconds. Default is 30</param>
    /// <param name="tryTimeoutSeconds">Try timeout in seconds. Default is 60</param>
    internal AzServiceBusMessagePublisher(string serviceBusNamespace, string queueOrTopicName, ILogger<IMessagePublisher<T>> logger,
        ServiceBusRetryMode retryMode = ServiceBusRetryMode.Exponential, int maxRetries = 3, int? delaySeconds = 1, 
        int? maxDelaySeconds = 30, int? tryTimeoutSeconds = 60)
    {
        var clientOptions = new ServiceBusClientOptions
        {
            RetryOptions = new ServiceBusRetryOptions
            {
                Mode = retryMode,
                MaxRetries = maxRetries,
                Delay = TimeSpan.FromSeconds(delaySeconds ?? 1),
                MaxDelay = TimeSpan.FromSeconds(maxDelaySeconds ?? 30),
                TryTimeout = TimeSpan.FromSeconds(tryTimeoutSeconds ?? 60)
            }
        };

        var credential = new DefaultAzureCredential();
        _serviceBusClient = new ServiceBusClient($"{serviceBusNamespace}.servicebus.windows.net", credential, clientOptions);
        _serviceBusSender = _serviceBusClient.CreateSender(queueOrTopicName);
        _logger = logger;
        _disposed = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzServiceBusMessagePublisher{T}"/> class using a shared access signature key for authentication.
    /// </summary>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueOrTopicName">The queue or topic name on the Service Bus</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="logger">The logger for the publisher</param>
    /// <param name="retryMode">The retry mode to use.  Default is Exponential</param>
    /// <param name="maxRetries">Maximum number of retries.  Default is 3</param>
    /// <param name="delaySeconds">Initial delay in seconds. Default is 1</param>
    /// <param name="maxDelaySeconds">Maximum delay in seconds. Default is 30</param>
    /// <param name="tryTimeoutSeconds">Try timeout in seconds. Default is 60</param>
    internal AzServiceBusMessagePublisher(string serviceBusNamespace, string queueOrTopicName, AzureNamedKeyCredential credential, ILogger<IMessagePublisher<T>> logger,
        ServiceBusRetryMode retryMode = ServiceBusRetryMode.Exponential, int maxRetries = 3, int? delaySeconds = 1,
        int? maxDelaySeconds = 30, int? tryTimeoutSeconds = 60)
    {
        var clientOptions = new ServiceBusClientOptions
        {
            RetryOptions = new ServiceBusRetryOptions
            {
                Mode = retryMode,
                MaxRetries = maxRetries,
                Delay = TimeSpan.FromSeconds(delaySeconds ?? 1),
                MaxDelay = TimeSpan.FromSeconds(maxDelaySeconds ?? 30),
                TryTimeout = TimeSpan.FromSeconds(tryTimeoutSeconds ?? 60)
            }
        };

        _serviceBusClient = new ServiceBusClient($"{serviceBusNamespace}.servicebus.windows.net", credential, clientOptions);        
        _serviceBusSender = _serviceBusClient.CreateSender(queueOrTopicName);
        _logger = logger;
        _disposed = false;
    }

    // --------------------------------------------------------------------------------------------
    // Methods
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Publishes a message to the Service Bus topic.
    /// </summary>
    /// <param name="messageBody">An object representing the message body</param>
    /// <param name="properties">Optional properties for the message</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task PublishMessageAsync(T messageBody, MessageProperties? properties = null)
    {
        var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(messageBody));
        serviceBusMessage.ContentType = "application/json";
        ApplyPropertiesToMessage(serviceBusMessage, properties);

        await _serviceBusSender.SendMessageAsync(serviceBusMessage);
    }


    /// <summary>
    /// Applies the specified properties to the Service Bus message.
    /// </summary>
    /// <param name="serviceBusMessage">The Service Bus message to which properties will be applied</param>
    /// <param name="properties">The properties to apply</param>
    private static void ApplyPropertiesToMessage(ServiceBusMessage serviceBusMessage, MessageProperties? properties)
    {
        if (properties == null)
            return;
        if (!string.IsNullOrWhiteSpace(properties.MessageId))
            serviceBusMessage.MessageId = properties.MessageId;
        if (!string.IsNullOrWhiteSpace(properties.Subject))
            serviceBusMessage.Subject = properties.Subject;
        if (!string.IsNullOrWhiteSpace(properties.CorrelationId))
            serviceBusMessage.CorrelationId = properties.CorrelationId;
        if (properties.ApplicationProperties != null && properties.ApplicationProperties.Any())
        {
            foreach (var kvp in properties.ApplicationProperties)
            {
                serviceBusMessage.ApplicationProperties[kvp.Key] = kvp.Value;
            }
        }
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
        
        await _serviceBusSender.DisposeAsync();
        await _serviceBusClient.DisposeAsync();
        GC.SuppressFinalize(this);        
    }


}

