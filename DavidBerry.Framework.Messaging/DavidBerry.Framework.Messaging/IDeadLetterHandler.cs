using DavidBerry.Framework.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging;

public interface IDeadLetterHandler<T> where T : class, new()
{
    public Task<Result<long>> GetMessageCountAsync(CancellationToken cancellationToken);

    public Task<Result<IReadOnlyList<DeadLetterMessage<T>>>> PeekMessagesAsync(int maxMessages, CancellationToken cancellationToken);

    public Task<Result> ResubmitMessageAsync(long sequenceNumber, CancellationToken cancellationToken);

    public Task<Result> ResubmitAllMessagesAsync(int maxMessages,CancellationToken cancellationToken);


    public Task<Result> DeleteMessageAsync(long sequenceNumber, CancellationToken cancellationToken); 
    
    public Task<Result> DeleteAllMessagesAsync(long sequenceNumber, CancellationToken cancellationToken);

}

