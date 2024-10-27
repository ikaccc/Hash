using System;
using BackendBlocks.MessageBroker.Consumer.Contracts;

namespace BackendBlocks.MessageBroker.Consumer;

public interface IMessageConsumerFactory
{
    IMessageConsumer Create();
}
