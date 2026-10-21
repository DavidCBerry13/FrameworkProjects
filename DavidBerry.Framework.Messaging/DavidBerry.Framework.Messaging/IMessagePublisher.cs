using DavidBerry.Framework.Functional;

namespace DavidBerry.Framework.Messaging;


public interface IMessagePublisher<T> where T : class, new()
{
    
    Task PublishMessageAsync(T messageBody, MessageProperties? properties = null);


}

