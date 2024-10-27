using System;
using BackendBlocks.MessageBroker.Consumer.Contracts;
using BackendBlocks.Messages.PublishSubscribeMessages;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace BackendBlocks.MessageBroker.Consumer;

public abstract class MessageConsumerProcessorBase : IMessageConsumerProcessor
{
    private readonly ILogger<MessageConsumerProcessorBase> _logger;
    private readonly IMessageConsumer _messageConsumer;

    public MessageConsumerProcessorBase(ILogger<MessageConsumerProcessorBase> logger, IMessageConsumer messageConsumer)
    {
        _logger = logger;
        _messageConsumer = messageConsumer;
    }

    public async Task RunAsync()
    {
        try
        {
            while (true)
            {
                var message = await _messageConsumer.ReceiveMessagesAsync().ConfigureAwait(false);

                if(message == null)
                {
                    //
                    return;
                }

                var handledMessages = await HandleMessagesAsync(message).ConfigureAwait(false);

                if (handledMessages != null)
                {
                    _messageConsumer.DeleteMessage(handledMessages);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
        }
    }

    protected abstract Task<ulong> HandleMessagesAsync(QueueMessage qMessages);   
}
