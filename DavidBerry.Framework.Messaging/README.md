# DavidBerry.Framework.Messaging Libraries

The libraries in this folder are intended to make it easier to write message or event based applications.  These libraries provide an abstraction layer over the underlying messaging infrastructure letting applications publish and subscribe to messages and events agnostic of the underlying messaging platform.  These libraries also simplify writing message or event based applications by abstracting away some of the implementation details.

Much of the terminology of these libraries are based around a Service Bus.  You _publish_ messages, you can _subscribe_ to messages, and messages that have errors are placed in a _dead letter queue_ for evaluation and potential reprocessing.  Not every messaging or eventing backend uses these terms or even directly implements these concepts. However, to create an abstraction layer that will work with any messaging backend, it is necessary to standardize both te terminology used and the behavior.  For example, Azure Storage Queues have no concept of a dead letter queue, so this is done using a second storage queue.

## DavidBerry.Framework.Messaging**

The **DavidBerry.Framework.Messaging** project defines a series of interfaces for publishing, subscribing to, and handling dead letter (error) messages.

- **IMessagePublisher** - Used to publish a message of type T to a messaging backend.
- **IMessageSubscriber** - Used to subscribe to a message of type T from a messaging backend.  You provide a handler function to handle the message.  If the handler function fails or throws an exception, the message is put into a dead letter queue.
- **IDeadLetterHandler** - Used to list and manage messages that have been placed in the dead letter queue due to some sort of error.

## DavidBerry.Framework.AzServiceBus

An implementation of the messaging framework using Azure Service Bus.  This implementation supports both point to point messaging using Azure Service Bus Queues and one to many (publish/subscribe) messaging using Azure Service Bus Topics.

## DavidBerry.Framework.AzStorageQueues

An implementation of the messaging framework using Azure Storage Queues.  Azure Storage Queues only support point to point messaging.

Azure Storage Queues offer the advantage of being more inexpensive than Azure Service Bus.  However, they are not as full featured.  As a result, some functionality natively present in Azure Service Bus has to be simulated in Azure Storage Queues.  Specifically, dead lettering is simulated using an additional Azure Storage Queue with the dead letter message wrapped in a JSON object to synthesize the dead letter information natively available in Service Bus.

All messages sent to Azure Storage Queues are serialized using JSON for simplicity, readability, and reliability.