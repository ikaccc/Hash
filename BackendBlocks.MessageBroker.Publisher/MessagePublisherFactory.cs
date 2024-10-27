using System;
using BackendBlocks.MessageBroker.Publisher.Contracts;
using BackendBlocks.QueueService;
using Microsoft.Extensions.Logging;

namespace BackendBlocks.MessageBroker.Publisher;

public class MessagePublisherFactory(IQueueClientFactory _queueClientFactory, Func<ILogger<MessagePublisher>> _getLogger) : IMessagePublisherFactory
{
    public IMessagePublisher Create()
    {
        return new MessagePublisher(_queueClientFactory, _getLogger());
    }
}
