using System;
using RabbitMQ.Client;

namespace BackendBlocks.QueueService;

public interface IQueueClientFactory
{
    IModel GetClient();
}
