using System.Security.Cryptography;
using BackendBlocks.Core.Contracts.Persistence;
using BackendBlocks.Core.Entities;
using BackendBlocks.MessageBroker.Publisher.Contracts;
using BackendBlocks.Messages.HashMessages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackendBlocks.Core.Features.Hashs.Commands.CreateHash;

public class CreateHashHandler(IAsyncRepository<Hash> _repository,
                                       //IMapper _mapper,
                                       IMessagePublisher _publisher,
                                       ILogger<CreateHashHandler> _logger) : IRequestHandler<CreateHashCommand, int>
{
    public async Task<int> Handle(CreateHashCommand request, CancellationToken cancellationToken)
    {
        await PublishHashesInBatchesAsync();
        return int.MinValue;
    }

    private async Task PublishHashesInBatchesAsync(int batchSize = 2000)
    {
        var batches = CreateBatches();

        // Send batches in parallel
        await Parallel.ForEachAsync(batches, async (batch, cancellationToken) =>
        {
            // Process each batch asynchronously
            //no need for concurent bag 
            //each batch’s tasks list exists only within the context of that batch’s parallel task
            var tasks = new List<Task>(); 

            foreach (var hash in batch)
            {
                tasks.Add(_publisher.PublishMessageAsync(hash));
            }

            // Wait for all messages in the batch to be published
            await Task.WhenAll(tasks);
        });
    }

    private List<List<CreateHashMessage>> CreateBatches(int batchSize = 2000)
    {
        var commands = new List<List<CreateHashMessage>>();
        var currentBatch = new List<CreateHashMessage>(batchSize); // Predefine size to improve performance
        var random = new Random();
        var sha1 = SHA1.Create();

        DateTimeOffset startDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset endDate = startDate.AddMonths(1).AddDays(-1); 

        for (int i = 0; i < 40000; i++)
        {
            // Generate a random byte array for the SHA-1 hash
            byte[] randomBytes = new byte[20]; // SHA-1 produces a 20-byte hash
            random.NextBytes(randomBytes);
            byte[] shaHash = sha1.ComputeHash(randomBytes);

            // Generate a random date in the range of 1 to end of month
            DateTimeOffset randomDate = new DateTimeOffset(startDate.Year, startDate.Month, random.Next(1, endDate.Day), 0, 0, 0, TimeSpan.Zero);

            // Add command to current batch
            currentBatch.Add(new CreateHashMessage{ShaHash = shaHash, Date = randomDate.DateTime});

            // If current batch is full, add to commands and reset
            if (currentBatch.Count == batchSize)
            {
                commands.Add(currentBatch);
                currentBatch = new List<CreateHashMessage>(batchSize); // Reset with predefined capacity
            }
        }

        // Add the remaining items if any
        if (currentBatch.Count > 0)
        {
            commands.Add(currentBatch);
        }

        return commands;
    }

}
