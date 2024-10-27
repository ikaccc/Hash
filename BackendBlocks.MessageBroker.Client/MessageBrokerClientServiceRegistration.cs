
using BackendBlocks.MessageBroker.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BackendBlocks.MessageBroker.Client;

public static class MessageBrokerClientServiceRegistration
{
    public static void AddMessageBrokerClientService(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBrokerClientFactory, MessageBrokerClientFactory>(s =>
        {
            var config = s.GetRequiredService<IOptions<MessageBrokerClientConfig>>().Value;
            return new MessageBrokerClientFactory(config);
        });
    }
}
