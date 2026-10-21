using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging;

public class DeadLetterMessage<T> where T : class, new()
{
    public long SequenceNumber { get; set; }

    public DateTimeOffset EnqueueTime { get; set; }

    public string? DeadLetterReason { get; set; }

    public string? DeadLetterErrorDescription { get; set; }

    public T? Body { get; set; }
}

