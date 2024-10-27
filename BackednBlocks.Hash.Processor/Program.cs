using System.Text.Json;
using BackednBlocks.Hash.Processor.HostedService;
using BackendBlocks.Core.Mappings;
using BackendBlocks.Infrastructure;
using BackendBlocks.MessageBroker.Client;
using BackendBlocks.MessageBroker.Client.Configuration;
using BackendBlocks.MessageBroker.Consumer;
using BackendBlocks.MessageBroker.Consumer.Contracts;
using BackendBlocks.QueueService;
using BackendBlocks.QueueService.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<MessageBrokerClientConfig>(builder.Configuration.GetSection(MessageBrokerClientConfig.SectionName));
builder.Services.Configure<QueueConfig>(builder.Configuration.GetSection(QueueConfig.SectionName));

builder.Services.AddInfrastructureServicesSingleton(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMessageBrokerClientService();
builder.Services.AddQueueService();
builder.Services.AddMessageConsumerService();

builder.Services.AddSingleton<IMessageConsumerProcessor, HashWorker>();
builder.Services.AddHostedService<HashService>();

builder.Services.AddSingleton(sp => MapperDecorator.CreateMapper());

builder.Services.AddOptions<JsonSerializerOptions>()
    .Configure<IServiceProvider>((options, serviceProvider) =>
    {
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

var host = builder.Build();
host.Run();

