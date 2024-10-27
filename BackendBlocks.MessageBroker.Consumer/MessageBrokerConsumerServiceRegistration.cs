using System;
using BackendBlocks.MessageBroker.Consumer.Contracts;
using BackendBlocks.MessageBroker.Consumer.Utilities;
using BackendBlocks.QueueService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackendBlocks.MessageBroker.Consumer;

public static class MessageBrokerConsumerServiceRegistration
{
    public static void AddMessageConsumerService(this IServiceCollection services)
    {
        services.AddSingleton<IMessageConsumerFactory, MessageConsumerFactory>(sp =>
        {
            var queueFactory = sp.GetRequiredService<IQueueClientFactory>();
            Func<ILogger<MessageConsumer>> getLoggerFunc = sp.GetRequiredService<ILogger<MessageConsumer>>;
            return new MessageConsumerFactory(queueFactory, getLoggerFunc);
        });

        services.AddSingleton<IMessageParser, MessageParser>();

        services.AddSingleton<IMessageConsumer, MessageConsumer>(sp =>
        {
            var messageConsumerFactory = sp.GetRequiredService<IMessageConsumerFactory>();
            return (MessageConsumer) messageConsumerFactory.Create();
        });
    }
}
