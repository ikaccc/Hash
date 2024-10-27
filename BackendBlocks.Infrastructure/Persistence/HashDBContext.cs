using System;
using BackendBlocks.Core.Common;
using BackendBlocks.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendBlocks.Infrastructure.Persistence;

public class HashDBContext(DbContextOptions<HashDBContext> options) : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hash>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "Ivan Cekov";
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}


