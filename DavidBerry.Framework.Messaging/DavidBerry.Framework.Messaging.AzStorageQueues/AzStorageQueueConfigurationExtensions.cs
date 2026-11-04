using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Messaging.AzStorageQueues;

public static class AzStorageQueueConfigurationExtensions
{
    // --------------------------------------------------------------------------------------------
    // Validation Helper Methods
    // --------------------------------------------------------------------------------------------

    private static void ValidateStorageAccountName(string storageAccountName)
    {
        if (string.IsNullOrWhiteSpace(storageAccountName))
        {
            throw new ArgumentException("Storage account name cannot be null or empty.", nameof(storageAccountName));
        }
    }

    private static void ValidateQueueName(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
        {
            throw new ArgumentException("Queue name cannot be null or empty.", nameof(queueName));
        }
    }

    // --------------------------------------------------------------------------------------------
    // Azure Storage Queue Message Publisher Configuration Extensions
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds an Azure Storage Queue message publisher to the DI container.
    /// Use this method when your application needs only one message publisher.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="queueName">The queue name in the storage account</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddStorageQueueMessagePublisher<T>(this IHostApplicationBuilder builder,
        string storageAccountName, string queueName) where T : class, new()
    {
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(queueName);

        builder.Services.AddSingleton<IMessagePublisher<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessagePublisher<T>(storageAccountName, queueName, logger);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Azure Storage Queue message publisher to the DI container.
    /// Use this method when your application needs multiple message publishers for different queues.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific publisher in the DI container</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="queueName">The queue name in the storage account</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedStorageQueueMessagePublisher<T>(this IHostApplicationBuilder builder,
        string key, string storageAccountName, string queueName) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(queueName);

        builder.Services.AddKeyedSingleton<IMessagePublisher<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessagePublisher<T>(storageAccountName, queueName, logger);
        });
        return builder;
    }


    /// <summary>
    /// Adds an Azure Storage Queue message publisher to the DI container.
    /// Use this method when your application needs only one message publisher.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="storageAccountKey">The Azure Storage account key</param>
    /// <param name="queueName">The queue name in the storage account</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddStorageQueueMessagePublisher<T>(this IHostApplicationBuilder builder,
        string storageAccountName, string queueName, string storageAccountKey) where T : class, new()
    {
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(queueName);

        builder.Services.AddSingleton<IMessagePublisher<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessagePublisher<T>(storageAccountName, queueName, storageAccountKey, logger);
        });
        return builder;
    }


    /// <summary>
    /// Adds a keyed Azure Storage Queue message publisher to the DI container.
    /// Use this method when your application needs multiple message publishers for different queues.
    /// </summary>
    /// <typeparam name="T">The message type being published</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific publisher in the DI container</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="queueName">The queue name in the storage account</param>
    /// <param name="storageAccountKey">The Azure Storage account key</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedStorageQueueMessagePublisher<T>(this IHostApplicationBuilder builder,
        string key, string storageAccountName, string queueName, string storageAccountKey) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(queueName);

        builder.Services.AddKeyedSingleton<IMessagePublisher<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessagePublisher<T>(storageAccountName, queueName, storageAccountKey, logger);
        });
        return builder;
    }


    // --------------------------------------------------------------------------------------------
    // Azure Storage Queue Message Subscriber Configuration Extensions
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds an Azure Storage Queue message subscriber to the DI container.
    /// Use this method when your application needs a single subscriber for a queue.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="receiveQueueName">The queue name to receive messages from</param>
    /// <param name="deadLetterQueueName">The queue name for dead letter messages</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddStorageQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string storageAccountName, string receiveQueueName, string deadLetterQueueName)
        where T : class, new() where K : class, new()
    {
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(receiveQueueName);
        ValidateQueueName(deadLetterQueueName);

        builder.Services.AddSingleton<IMessageSubscriber<T, K>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessageSubscriber<T, K>(storageAccountName, receiveQueueName, deadLetterQueueName, logger);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Azure Storage Queue message subscriber to the DI container.
    /// Use this method when your application needs multiple subscribers for different queues.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific subscriber in the DI container</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="receiveQueueName">The queue name to receive messages from</param>
    /// <param name="deadLetterQueueName">The queue name for dead letter messages</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedStorageQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string key, string storageAccountName, string receiveQueueName, string deadLetterQueueName)
        where T : class, new() where K : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(receiveQueueName);
        ValidateQueueName(deadLetterQueueName);

        builder.Services.AddKeyedSingleton<IMessageSubscriber<T, K>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessageSubscriber<T, K>(storageAccountName, receiveQueueName, deadLetterQueueName, logger);
        });
        return builder;
    }


    /// <summary>
    /// Adds an Azure Storage Queue message subscriber to the DI container.
    /// Use this method when your application needs a single subscriber for a queue.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="receiveQueueName">The queue name to receive messages from</param>
    /// <param name="deadLetterQueueName">The queue name for dead letter messages</param>
    /// <param name="storageAccountKey">The Azure Storage account key</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddStorageQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string storageAccountName, string receiveQueueName, string deadLetterQueueName, string storageAccountKey)
        where T : class, new() where K : class, new()
    {
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(receiveQueueName);
        ValidateQueueName(deadLetterQueueName);

        builder.Services.AddSingleton<IMessageSubscriber<T, K>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessageSubscriber<T, K>(storageAccountName, receiveQueueName, deadLetterQueueName, storageAccountKey, logger);
        });
        return builder;
    }


    /// <summary>
    /// Adds a keyed Azure Storage Queue message subscriber to the DI container.
    /// Use this method when your application needs multiple subscribers for different queues.
    /// </summary>
    /// <typeparam name="T">The message type being subscribed to</typeparam>
    /// <typeparam name="K">The handler result type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific subscriber in the DI container</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="receiveQueueName">The queue name to receive messages from</param>
    /// <param name="deadLetterQueueName">The queue name for dead letter messages</param>
    /// <param name="storageAccountKey">The Azure Storage account key</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedStorageQueueSubscriber<T, K>(this IHostApplicationBuilder builder,
        string key, string storageAccountName, string receiveQueueName, string deadLetterQueueName, string storageAccountKey)
        where T : class, new() where K : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(receiveQueueName);
        ValidateQueueName(deadLetterQueueName);

        builder.Services.AddKeyedSingleton<IMessageSubscriber<T, K>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IMessagePublisher<T>>>();
            return new AzStorageQueueMessageSubscriber<T, K>(storageAccountName, receiveQueueName, deadLetterQueueName, storageAccountKey, logger);
        });
        return builder;
    }


    // --------------------------------------------------------------------------------------------
    // Azure Storage Queue Dead Letter Handler Configuration Extensions
    // --------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds an Azure Storage Queue dead letter handler to the DI container.
    /// Use this method when your application needs a single dead letter handler for a queue.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="deadLetterQueueName">The queue name for dead letter messages</param>
    /// <param name="receiveQueueName">The queue name to resubmit messages to</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddStorageQueueDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string storageAccountName, string deadLetterQueueName, string receiveQueueName) where T : class, new()
    {
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(deadLetterQueueName);
        ValidateQueueName(receiveQueueName);

        builder.Services.AddSingleton<IDeadLetterHandler<T>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzStorageQueueDeadLetterHandler<T>(storageAccountName, deadLetterQueueName, receiveQueueName, logger);
        });
        return builder;
    }

    /// <summary>
    /// Adds a keyed Azure Storage Queue dead letter handler to the DI container.
    /// Use this method when your application needs multiple dead letter handlers for different queues.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="builder">The host application builder</param>
    /// <param name="key">The key to identify this specific dead letter handler in the DI container</param>
    /// <param name="storageAccountName">The Azure Storage account name</param>
    /// <param name="deadLetterQueueName">The queue name for dead letter messages</param>
    /// <param name="receiveQueueName">The queue name to resubmit messages to</param>
    /// <returns>The host application builder for chaining</returns>
    public static IHostApplicationBuilder AddKeyedStorageQueueDeadLetterHandler<T>(this IHostApplicationBuilder builder,
        string key, string storageAccountName, string deadLetterQueueName, string receiveQueueName) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        ValidateStorageAccountName(storageAccountName);
        ValidateQueueName(deadLetterQueueName);
        ValidateQueueName(receiveQueueName);

        builder.Services.AddKeyedSingleton<IDeadLetterHandler<T>>(key, (sp, serviceKey) =>
        {
            var logger = sp.GetRequiredService<ILogger<IDeadLetterHandler<T>>>();
            return new AzStorageQueueDeadLetterHandler<T>(storageAccountName, deadLetterQueueName, receiveQueueName, logger);
        });
        return builder;
    }
}

