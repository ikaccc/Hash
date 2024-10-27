using System;
using BackendBlocks.MessageBroker.Consumer.Contracts;

namespace BackednBlocks.Hash.Processor.HostedService;

public class HashService : BackgroundService
{
    private readonly ILogger<HashService> _logger;
    private readonly IMessageConsumerProcessor _worker;

    public HashService(ILogger<HashService> logger, IMessageConsumerProcessor worker)
    {
        _logger = logger;
        _worker = worker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Hash Service processor worker started..");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _worker.RunAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Hash Processor Service");
        }

        _logger.LogInformation("Hash Service processor worker shutdown.");
    }
}
