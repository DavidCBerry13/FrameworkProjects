using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Messaging;

public interface IMessageSerializer<T, TOut> where T : class, new() where TOut : class, new()
{
    TOut Serialize(T message);
    T Deserialize(TOut message);
}


