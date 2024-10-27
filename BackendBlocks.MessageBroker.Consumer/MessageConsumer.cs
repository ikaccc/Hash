using System.Text;
using System.Threading.Channels;
using BackendBlocks.MessageBroker.Consumer.Contracts;
using BackendBlocks.Messages.PublishSubscribeMessages;
using BackendBlocks.QueueService;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackendBlocks.MessageBroker.Consumer;

public class MessageConsumer : IMessageConsumer
{
    private readonly IModel _channel;
    private readonly ILogger<MessageConsumer> _logger;
    private readonly AsyncEventingBasicConsumer _consumer;

    public MessageConsumer(
        IQueueClientFactory queueClientFactory,
        ILogger<MessageConsumer> logger)
    {
        _channel = queueClientFactory.GetClient();
        _consumer = new AsyncEventingBasicConsumer(_channel);
        _channel.BasicQos(0, 10, false);
        _logger = logger;
    }

    //I initially thought RabbitMQ had pooling capabilities similar to AWS SQL or Azure Service Bus, 
    //where you can control the number of messages pulled at once, and I began the implementation with that 
    //assumption in mind. However, as I started working on the consumer, I realized that RabbitMQ only allows 
    //pulling one message at a time. Given the time constraints, I’ll leave the code as it is, as I don’t have 
    //enough time to make further changes.
    public async Task<QueueMessage?> ReceiveMessagesAsync()
    {
        return await Task.Run(() =>
        {
            var result = _channel.BasicGet(queue: _channel.CurrentQueue, autoAck: false);
            if (result == null)
            {
                return null;
            }

            return new QueueMessage(Encoding.UTF8.GetString(result.Body.ToArray()), result.BasicProperties.MessageId, result.BasicProperties.Type, result.DeliveryTag);
        });
    }

    //this could be right implementation, but as i said, i noticed to late
    public async Task StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);
        var channel = Channel.CreateUnbounded<BasicDeliverEventArgs>(); // Use channel to process messages

        consumer.Received += async (model, ea) =>
        {
            await channel.Writer.WriteAsync(ea);
        };

        _channel.BasicConsume(queue: _channel.CurrentQueue, autoAck: false, consumer: consumer);

        // Process messages in parallel using 4 threads
        await Parallel.ForEachAsync(Enumerable.Range(0, 4), async (i, _) =>
        {
            while (await channel.Reader.WaitToReadAsync())
            {
                if (channel.Reader.TryRead(out var ea))
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[Thread {i}] Received: {message}");

                    /*Processing the message*/

                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            }
        });
    }

    public void DeleteMessage(ulong delivereTag)
    {
        _channel.BasicAck(deliveryTag: delivereTag, multiple: false);
    }
}
