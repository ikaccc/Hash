using System;
using BackendBlocks.MessageBroker.Client.Configuration;
using RabbitMQ.Client;

namespace BackendBlocks.MessageBroker.Client;

public class MessageBrokerClientFactory : IMessageBrokerClientFactory
{
    private readonly IMessageBrokerClientConfig _config;

    public MessageBrokerClientFactory(IMessageBrokerClientConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public IConnection Create()
    {
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password,
            Port = _config.Port,
            AutomaticRecoveryEnabled = true,  
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10) 
        };

        return factory.CreateConnection();
    }
}
