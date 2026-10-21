using DavidBerry.Framework.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging;

public interface IMessageSubscriber<T, K> where T : class, new()
{

    Task SubscribeAsync(Func<T, MessageContext, Task<Result<K>>> messagehandler, CancellationToken cancellationToken);


    Task StopAsync();

}

