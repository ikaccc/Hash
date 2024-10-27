using System;

namespace BackendBlocks.MessageBroker.Publisher.Contracts;

public interface IMessagePublisherFactory
{
    IMessagePublisher Create();
}
