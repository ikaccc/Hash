using System;

namespace BackendBlocks.QueueService.Configuration;

public interface IQueueConfig
{
    string Name { get; }
    bool Durable { get; }
    bool AutoDelete { get; }
    bool Exclusive { get; }

}
