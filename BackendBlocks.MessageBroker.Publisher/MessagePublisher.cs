using System;
using System.Text;
using System.Text.Json;
using BackendBlocks.MessageBroker.Publisher.Contracts;
using BackendBlocks.Messages.PublishSubscribeMessages;
using BackendBlocks.Messages.PublishSubscribeMessages.Contracts;
using BackendBlocks.QueueService;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BackendBlocks.MessageBroker.Publisher;

public class MessagePublisher : IMessagePublisher
{
    private readonly ILogger<MessagePublisher> _logger;
    private readonly IModel _channel;

    public MessagePublisher(IQueueClientFactory queueClientFactory,
    ILogger<MessagePublisher> logger)
    {
        _logger = logger;
        _channel = queueClientFactory.GetClient();
    }

    public async Task PublishMessageAsync<TMessage>(TMessage message) where TMessage : IEventMessage
    {
        try 
        {
            string jsonMessage = JsonSerializer.Serialize(message);

            var basicProperties = _channel.CreateBasicProperties();
            basicProperties.Type = message.GetType().Name;

            await Task.Run(() => 
            {
                _channel.BasicPublish(exchange: "", routingKey: _channel.CurrentQueue, basicProperties: basicProperties, body: Encoding.UTF8.GetBytes(jsonMessage));
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message");
            throw;
        }   
    }
}
