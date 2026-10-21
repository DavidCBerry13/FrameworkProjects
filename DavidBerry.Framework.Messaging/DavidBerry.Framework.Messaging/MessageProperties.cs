using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging;

public class MessageProperties
{

    public string? MessageId { get; init; }

    public string? Subject { get; init; }

    public string? CorrelationId { get; init; }

    public IReadOnlyDictionary<string, object> ApplicationProperties = new Dictionary<string, object>();


    public static MessageProperties Create(string? messageId = null, string? subject = null, string? correlationId = null, IReadOnlyDictionary<string, object>? applicationProperties = null)
    {
        return new MessageProperties
        {
            MessageId = messageId,
            Subject = subject,
            CorrelationId = correlationId,
            ApplicationProperties = applicationProperties ?? new Dictionary<string, object>()
        };
    }
}

