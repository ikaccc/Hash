using System;
using BackendBlocks.MessageBroker.Publisher.Contracts;
using BackendBlocks.QueueService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BackendBlocks.MessageBroker.Publisher;

public static class MessageBrokerPublisherServiceRegistration
{
    public static void AddMessagePublisherService(this IServiceCollection services)
    {
        services.AddSingleton<IMessagePublisherFactory, MessagePublisherFactory>(sp =>
        {
            var queueFactory = sp.GetRequiredService<IQueueClientFactory>();
            Func<ILogger<MessagePublisher>> getLoggerFunction = sp.GetRequiredService<ILogger<MessagePublisher>>;

            return new MessagePublisherFactory(queueFactory, getLoggerFunction);
        });

        services.AddSingleton<IMessagePublisher, MessagePublisher>(sp =>
        {
            var msgPubFactory = sp.GetRequiredService<IMessagePublisherFactory>();
            return (MessagePublisher)msgPubFactory.Create();
        });
    }
}
