using System;
using BackendBlocks.QueueService.Configuration;
using RabbitMQ.Client;

namespace BackendBlocks.QueueService;

public class QueueClientFactory : IQueueClientFactory
{
    private readonly IQueueConfig _config;
    private readonly IConnection _connection;

    public QueueClientFactory(IQueueConfig config, IConnection connection)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public IModel GetClient()
    {
        var channel = _connection.CreateModel();

        channel.QueueDeclare(queue: _config.Name, durable: _config.Durable, exclusive: _config.Exclusive, autoDelete: _config.AutoDelete, arguments: null);
        
        return channel;
    }
}
