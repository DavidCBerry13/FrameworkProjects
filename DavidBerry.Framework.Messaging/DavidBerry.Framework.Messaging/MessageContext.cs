using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging;

public class MessageContext
{

    public required string MessageId { get; init; }

    public required long SequenceNumber { get; init; }

    public required int DeliveryCount { get; init; }

    public required DateTimeOffset EnqueueTime { get; init; }

}

