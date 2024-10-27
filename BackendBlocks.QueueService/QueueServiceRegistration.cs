using BackendBlocks.MessageBroker.Client;
using BackendBlocks.QueueService.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BackendBlocks.QueueService;

public static class QueueServiceRegistration
{
    public static void AddQueueService(this IServiceCollection services)
    {
        services.AddSingleton<IQueueClientFactory, QueueClientFactory>(s =>
        {
            var config = s.GetRequiredService<IOptions<QueueConfig>>().Value;
            var brokerClientFac = s.GetRequiredService<IMessageBrokerClientFactory>();
            return new QueueClientFactory(config, brokerClientFac.Create());
        });
    }
}
