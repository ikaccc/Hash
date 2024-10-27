using System.Text.Json;
using BackendBlocks.Core;
using BackendBlocks.Infrastructure;
using BackendBlocks.MessageBroker.Client;
using BackendBlocks.MessageBroker.Client.Configuration;
using BackendBlocks.MessageBroker.Publisher;
using BackendBlocks.QueueService;
using BackendBlocks.QueueService.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageBrokerClientConfig>(builder.Configuration.GetSection(MessageBrokerClientConfig.SectionName));
builder.Services.Configure<QueueConfig>(builder.Configuration.GetSection(QueueConfig.SectionName));

builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMessageBrokerClientService();
builder.Services.AddQueueService();
builder.Services.AddMessagePublisherService();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.IncludeFields = true;
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

