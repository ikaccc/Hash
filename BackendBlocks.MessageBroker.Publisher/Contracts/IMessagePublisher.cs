using System;
using BackendBlocks.Messages.PublishSubscribeMessages.Contracts;

namespace BackendBlocks.MessageBroker.Publisher.Contracts;

public interface IMessagePublisher
{
    Task PublishMessageAsync<TMessage>(TMessage message) where TMessage : IEventMessage;
}
