using System;

namespace BackendBlocks.MessageBroker.Consumer.Contracts;

public interface IMessageConsumerProcessor
{
    Task RunAsync();
}
