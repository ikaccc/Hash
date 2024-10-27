using System;
using BackendBlocks.Messages.PublishSubscribeMessages;

namespace BackendBlocks.MessageBroker.Consumer.Contracts;

public interface IMessageConsumer
{
    Task<QueueMessage> ReceiveMessagesAsync();
    void DeleteMessage(ulong delivereTag);
}
