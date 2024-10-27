using System;
using BackendBlocks.Messages.PublishSubscribeMessages;
using BackendBlocks.Messages.PublishSubscribeMessages.Contracts;

namespace BackendBlocks.MessageBroker.Consumer.Utilities;

public interface IMessageParser
{
    DeserializedQueueMessage<TMessage> Parse<TMessage>(QueueMessage queueMessage)
        where TMessage : IEventMessage;
}
