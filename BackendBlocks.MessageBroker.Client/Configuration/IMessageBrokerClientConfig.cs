using System;

namespace BackendBlocks.MessageBroker.Client.Configuration;

public interface IMessageBrokerClientConfig
{
    string HostName { get; }
    string UserName { get; }
    string Password { get; }
    int Port { get; }
}
