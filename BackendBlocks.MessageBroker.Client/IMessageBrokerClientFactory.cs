using System;
using RabbitMQ.Client;

namespace BackendBlocks.MessageBroker.Client;

public interface IMessageBrokerClientFactory
{
    IConnection Create();
}
