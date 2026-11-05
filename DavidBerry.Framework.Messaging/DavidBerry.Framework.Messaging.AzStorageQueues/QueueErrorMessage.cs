using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Messaging.AzStorageQueues;

/// <summary>
/// Represents an error message sent to the dead letter queue when message processing fails.
/// This is used internally by the Storage Queue subscriber to communicate errors.
/// </summary>
internal class QueueErrorMessage
{
    public long SequenceNumber { get; set; }

    public DateTimeOffset EnqueueTime { get; set; }

    public string? DeadLetterReason { get; set; }

    public string? DeadLetterErrorDescription { get; set; }

    public string MessageBody { get; set; } = string.Empty;
}
