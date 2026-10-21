using Azure;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging.AzServiceBus;

public static class AzServiceBusConfigurationExtensions
{
    // --------------------------------------------------------------------------------------------
    // Validation Helper Methods
    // --------------------------------------------------------------------------------------------

    private static void ValidateServiceBusNamespace(string serviceBusNamespace)
    {
        if (string.IsNullOrWhiteSpace(serviceBusNamespace))
        {
            throw new ArgumentException("Service Bus namespace cannot be null or empty.", nameof(serviceBusNamespace));
        }
    }

    private static void ValidateTopicName(string topicName)
    {
        if (string.IsNullOrWhiteSpace(topicName))
        {
            throw new ArgumentException("Topic name cannot be null or empty.", nameof(topicName));
        }
    }

    private static void ValidateQueueName(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
        {
            throw new ArgumentException("Queue name cannot be null or empty.", nameof(queueName));
        }
    }

    private static void ValidateSubscriptionName(string subscriptionName)
    {
        if (string.IsNullOrWhiteSpace(subscriptionName))
        {
            throw new ArgumentException("Subscription name cannot be null or empty.", nameof(subscriptionName));
        }
    }

    // --------------------------------------------------------------------------------------------
    // Service Bus Message Publisher Configuration Extensions
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds a Service Bus message publisher to the DI container.
    /// Use this method when your application needs only one message publisher.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueOrTopicName">The topic name on the Service Bus</param>
    /// <param name="retryMode">The retry mode to use (default: Exponential)</param>
    /// <param name="maxRetries">Maximum number of retries (default: 3)</param>
    /// <param name="delaySeconds">Initial delay in seconds (default: 1)</param>
    /// <param name="maxDelaySeconds">Maximum delay in seconds (default: 30)</param>
    /// <param name="tryTimeoutSeconds">Try timeout in seconds (default: 60)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusMessagePublisher<T>(this IHostApplicationBuilder builder, string serviceBusNamespace, string queueOrTopicName, 
        ServiceBusRetryMode retryMode = ServiceBusRetryMode.Exponential, int maxRetries = 3, 
        int? delaySeconds = 1, int? maxDelaySeconds = 30, int? tryTimeoutSeconds = 60) where T : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(queueOrTopicName);

        builder.Services.AddSingleton<IMessagePublisher<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzServiceBusMessagePublisher<T>(serviceBusNamespace, queueOrTopicName, logger, retryMode, maxRetries, delaySeconds, maxDelaySeconds, tryTimeoutSeconds);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus message publisher to the DI container.
    /// Use this method when your application needs multiple message publishers for different topics.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific publisher in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueOrTopicName">The topic name on the Service Bus</param>
    /// <param name="retryMode">The retry mode to use (default: Exponential)</param>
    /// <param name="maxRetries">Maximum number of retries (default: 3)</param>
    /// <param name="delaySeconds">Initial delay in seconds (default: 1)</param>
    /// <param name="maxDelaySeconds">Maximum delay in seconds (default: 30)</param>
    /// <param name="tryTimeoutSeconds">Try timeout in seconds (default: 60)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusMessagePublisher<T>(this IHostApplicationBuilder builder, string key, string serviceBusNamespace, string queueOrTopicName,
        ServiceBusRetryMode retryMode = ServiceBusRetryMode.Exponential, int maxRetries = 3,
        int? delaySeconds = 1, int? maxDelaySeconds = 30, int? tryTimeoutSeconds = 60) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(queueOrTopicName);

        builder.Services.AddKeyedSingleton<IMessagePublisher<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzServiceBusMessagePublisher<T>(serviceBusNamespace, queueOrTopicName, logger, retryMode, maxRetries, delaySeconds, maxDelaySeconds, tryTimeoutSeconds);
        });
        return builder;
    }


    /// <summary>
    /// Adds a Service Bus message publisher using Named Key credentials to the DI container.
    /// Use this method when your application needs only one message publisher authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="retryMode">The retry mode to use (default: Exponential)</param>
    /// <param name="maxRetries">Maximum number of retries (default: 3)</param>
    /// <param name="delaySeconds">Initial delay in seconds (default: 1)</param>
    /// <param name="maxDelaySeconds">Maximum delay in seconds (default: 30)</param>
    /// <param name="tryTimeoutSeconds">Try timeout in seconds (default: 60)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusMessagePublisher<T>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string topicName, AzureNamedKeyCredential credential,
        ServiceBusRetryMode retryMode = ServiceBusRetryMode.Exponential, int maxRetries = 3,
        int? delaySeconds = 1, int? maxDelaySeconds = 30, int? tryTimeoutSeconds = 60) where T : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddSingleton<IMessagePublisher<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzServiceBusMessagePublisher<T>(serviceBusNamespace, topicName, credential, logger,
                retryMode, maxRetries, delaySeconds, maxDelaySeconds, tryTimeoutSeconds);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus message publisher using Named Key credentials to the DI container.
    /// Use this method when your application needs multiple message publishers for different topics, authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific publisher in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="retryMode">The retry mode to use (default: Exponential)</param>
    /// <param name="maxRetries">Maximum number of retries (default: 3)</param>
    /// <param name="delaySeconds">Initial delay in seconds (default: 1)</param>
    /// <param name="maxDelaySeconds">Maximum delay in seconds (default: 30)</param>
    /// <param name="tryTimeoutSeconds">Try timeout in seconds (default: 60)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusMessagePublisher<T>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string topicName, AzureNamedKeyCredential credential,
        ServiceBusRetryMode retryMode = ServiceBusRetryMode.Exponential, int maxRetries = 3,
        int? delaySeconds = 1, int? maxDelaySeconds = 30, int? tryTimeoutSeconds = 60) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddKeyedSingleton<IMessagePublisher<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzServiceBusMessagePublisher<T>(serviceBusNamespace, topicName, credential, logger,
                retryMode, maxRetries, delaySeconds, maxDelaySeconds, tryTimeoutSeconds);
        });
        return builder;
    }


    // --------------------------------------------------------------------------------------------
    // Service Bus Message Queue Subscriber Configuration Extensions
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds a Service Bus message subscriber for queues to the DI container.
    /// Use this method when your application needs a single subscriber for a queue.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string queueName,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
        where T : class, new() where K : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);

        builder.Services.AddSingleton<IMessageSubscriber<T, K>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, queueName, logger,
                maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus message subscriber for queues to the DI container.
    /// Use this method when your application needs multiple subscribers for different queues.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific subscriber in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string queueName,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
        where T : class, new() where K : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);

        builder.Services.AddKeyedSingleton<IMessageSubscriber<T, K>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, queueName, logger,
                maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }

    /// <summary>
    /// Adds a Service Bus message subscriber for queues using Named Key credentials to the DI container.
    /// Use this method when your application needs a single subscriber for a queue, authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string queueName, AzureNamedKeyCredential credential,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
        where T : class, new() where K : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddSingleton<IMessageSubscriber<T, K>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, queueName, credential, logger,
                maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus message subscriber for queues using Named Key credentials to the DI container.
    /// Use this method when your application needs multiple subscribers for different queues, authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific subscriber in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string queueName, AzureNamedKeyCredential credential,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
        where T : class, new() where K : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddKeyedSingleton<IMessageSubscriber<T, K>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, queueName, credential, logger,
                maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }

    // --------------------------------------------------------------------------------------------
    // Service Bus Message Topic Subscriber Configuration Extensions
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds a Service Bus message subscriber for topics and subscriptions to the DI container.
    /// Use this method when your application needs a single subscriber for a topic subscription.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusTopicSubscriber<T, K>(this IHostApplicationBuilder builder, 
        string serviceBusNamespace, string topicName, string subscriptionName,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock) 
        where T : class, new() where K : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);

        builder.Services.AddSingleton<IMessageSubscriber<T, K>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, topicName, subscriptionName, logger, 
                maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus message subscriber for topics and subscriptions to the DI container.
    /// Use this method when your application needs multiple subscribers for different topic subscriptions.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific subscriber in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusTopicSubscriber<T, K>(this IHostApplicationBuilder builder, 
        string key, string serviceBusNamespace, string topicName, string subscriptionName,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock) 
        where T : class, new() where K : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);

        builder.Services.AddKeyedSingleton<IMessageSubscriber<T, K>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, topicName, subscriptionName, logger, 
                maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }

    /// <summary>
    /// Adds a Service Bus message subscriber for topics and subscriptions using Named Key credentials to the DI container.
    /// Use this method when your application needs a single subscriber for a topic subscription, authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusTopicSubscriber<T, K>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string topicName, string subscriptionName, AzureNamedKeyCredential credential,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
        where T : class, new() where K : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddSingleton<IMessageSubscriber<T, K>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, topicName, subscriptionName, credential, logger,
                autoCompleteMessages: true, maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus message subscriber for topics and subscriptions using Named Key credentials to the DI container.
    /// Use this method when your application needs multiple subscribers for different topic subscriptions, authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific subscriber in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <param name="maxConcurrentCalls">Maximum number of concurrent message processing calls (default: 1)</param>
    /// <param name="prefetchCount">Number of messages to prefetch (default: 0)</param>
    /// <param name="receiveMode">The receive mode to use (default: PeekLock)</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusTopicSubscriber<T, K>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string topicName, string subscriptionName, AzureNamedKeyCredential credential,
        int maxConcurrentCalls = 1, int prefetchCount = 0, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
        where T : class, new() where K : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddKeyedSingleton<IMessageSubscriber<T, K>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessageSubscriber<T, K>>>();
            return new AzServiceBusMessageSubscriber<T, K>(serviceBusNamespace, topicName, subscriptionName, credential, logger,
                autoCompleteMessages: true, maxConcurrentCalls: maxConcurrentCalls, prefetchCount: prefetchCount, receiveMode: receiveMode);
        });
        return builder;
    }


    // --------------------------------------------------------------------------------------------
    // Service Bus Dead Letter Handler Configuration Extensions (Queues)
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds a Service Bus dead letter handler for queues to the DI container.
    /// Use this method when your application needs a single dead letter handler for a queue.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusQueueDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string queueName) where T : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);

        builder.Services.AddSingleton<IDeadLetterHandler<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, queueName, logger);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus dead letter handler for queues to the DI container.
    /// Use this method when your application needs multiple dead letter handlers for different queues.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific dead letter handler in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusQueueDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string queueName) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);

        builder.Services.AddKeyedSingleton<IDeadLetterHandler<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, queueName, logger);
        });
        return builder;
    }


    /// <summary>
    /// Adds a Service Bus dead letter handler for queues using SAS credentials to the DI container.
    /// Use this method when your application needs a single dead letter handler for a queue, authenticated with a Shared Access Signature.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="sasCredential">A Shared Access Signature (SAS) credential with Listen, Send, and Manage permissions</param>
    /// <summary>
    /// Adds a Service Bus dead letter handler for queues using SAS credentials to the DI container.
    /// Use this method when your application needs a single dead letter handler for a queue, authenticated with a Shared Access Signature.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="sasCredential">A Shared Access Signature (SAS) credential with Listen, Send, and Manage permissions</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusQueueDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string queueName, AzureNamedKeyCredential credential) where T : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddSingleton<IDeadLetterHandler<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, queueName, credential, logger);
        });
        return builder;
    }


    /// <summary>
    /// Adds a keyed Service Bus dead letter handler for queues using SAS credentials to the DI container.
    /// Use this method when your application needs multiple dead letter handlers for different queues, authenticated with a Shared Access Signature.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific dead letter handler in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="queueName">The queue name on the Service Bus</param>
    /// <param name="sasCredential">A Shared Access Signature (SAS) credential with Listen, Send, and Manage permissions</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusQueueDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string queueName, AzureNamedKeyCredential credential) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateQueueName(queueName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddKeyedSingleton<IDeadLetterHandler<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, queueName, credential, logger);
        });
        return builder;
    }

    // --------------------------------------------------------------------------------------------
    // Service Bus Dead Letter Handler Configuration Extensions (Topics and Subscriptions)
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds a Service Bus dead letter handler for topics and subscriptions to the DI container.
    /// Use this method when your application needs a single dead letter handler for a topic subscription.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusTopicDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string topicName, string subscriptionName) where T : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);

        builder.Services.AddSingleton<IDeadLetterHandler<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, topicName, subscriptionName, logger);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus dead letter handler for topics and subscriptions to the DI container.
    /// Use this method when your application needs multiple dead letter handlers for different topic subscriptions.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific dead letter handler in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusTopicDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string topicName, string subscriptionName) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);

        builder.Services.AddKeyedSingleton<IDeadLetterHandler<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, topicName, subscriptionName, logger);
        });
        return builder;
    }


    /// <summary>
    /// Adds a Service Bus dead letter handler for topics and subscriptions using Named Key credentials to the DI container.
    /// Use this method when your application needs a single dead letter handler for a topic subscription, authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddServiceBusTopicDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string serviceBusNamespace, string topicName, string subscriptionName, AzureNamedKeyCredential credential) where T : class, new()
    {
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddSingleton<IDeadLetterHandler<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, topicName, subscriptionName, credential, logger);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Service Bus dead letter handler for topics and subscriptions using Named Key credentials to the DI container.
    /// Use this method when your application needs multiple dead letter handlers for different topic subscriptions, authenticated with an AzureNamedKeyCredential.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific dead letter handler in the DI container</param>
    /// <param name="serviceBusNamespace">The Service Bus namespace</param>
    /// <param name="topicName">The topic name on the Service Bus</param>
    /// <param name="subscriptionName">The subscription name on the topic</param>
    /// <param name="credential">An AzureNamedKeyCredential used to authenticate to the service bus</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedServiceBusTopicDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string key, string serviceBusNamespace, string topicName, string subscriptionName, AzureNamedKeyCredential credential) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateServiceBusNamespace(serviceBusNamespace);
        ValidateTopicName(topicName);
        ValidateSubscriptionName(subscriptionName);
        if (credential == null)
        {
            throw new ArgumentNullException(nameof(credential), "Credential cannot be null.");
        }

        builder.Services.AddKeyedSingleton<IDeadLetterHandler<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzServiceBusDeadLetterHandler<T>(serviceBusNamespace, topicName, subscriptionName, credential, logger);
        });
        return builder;
    }


}

