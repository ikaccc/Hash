using System;

namespace BackendBlocks.QueueService.Configuration;

public class QueueConfig : IQueueConfig
{
    public static string SectionName = nameof(QueueConfig);
    public string Name { get; set; } = string.Empty;
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public bool Exclusive { get; set; }
}
