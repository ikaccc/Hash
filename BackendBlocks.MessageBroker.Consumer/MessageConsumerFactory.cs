using BackendBlocks.MessageBroker.Consumer.Contracts;
using BackendBlocks.QueueService;
using Microsoft.Extensions.Logging;

namespace BackendBlocks.MessageBroker.Consumer;

public class MessageConsumerFactory(
        IQueueClientFactory _queueClientFactory,
        Func<ILogger<MessageConsumer>> _getLogger) : IMessageConsumerFactory
{
    public IMessageConsumer Create()
    {
        return new MessageConsumer(_queueClientFactory, _getLogger());
    }
}
