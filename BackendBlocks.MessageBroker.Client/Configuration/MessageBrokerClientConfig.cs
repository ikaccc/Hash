using System;

namespace BackendBlocks.MessageBroker.Client.Configuration;

public class MessageBrokerClientConfig : IMessageBrokerClientConfig
{
    public static string SectionName = "RabbitMq";
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; }
}
