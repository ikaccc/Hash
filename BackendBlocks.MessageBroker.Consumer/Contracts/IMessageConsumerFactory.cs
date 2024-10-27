using System;

namespace BackendBlocks.MessageBroker.Consumer.Contracts;

public interface IMessageConsumerFactory
{
    IMessageConsumer Create();
}
