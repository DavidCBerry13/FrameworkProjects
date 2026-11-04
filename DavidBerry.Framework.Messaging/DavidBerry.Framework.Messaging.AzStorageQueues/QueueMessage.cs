using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Messaging.AzStorageQueues;

internal class QueueMessage<T> where T : class, new()
{

    public MessageProperties Properties { get; set; } = new MessageProperties();

    public T Body { get; set; } = new T();

}

