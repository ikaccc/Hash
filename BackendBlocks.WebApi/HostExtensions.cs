using System;
using Microsoft.EntityFrameworkCore;

namespace BackendBlocks.WebApi;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host,
        //Action<TContext, IServiceProvider> seeder,
        int retry = 0) 
        where TContext : DbContext
    {
        var retryForAvailability = retry;

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger.LogInformation("migrating started for sql server");
            InvokeSeeder(context, services);
            logger.LogInformation("migrating finished for sql server");
        }
        catch (Exception ex)
        {
            logger.LogError($"migrating errored for sql server, {ex.Message}");
            if (retryForAvailability < 50)
            {
                //MigrateDatabase(host, retryForAvailability + 1);
            }
            throw;
        }

        return host;
    }

    private static void InvokeSeeder<TContext>(
        //Action<TContext, IServiceProvider> seeder, used this for testing perpuse
        TContext context, 
        IServiceProvider services) 
        where TContext : DbContext
    {
        context.Database.Migrate();
        //seeder(context, services);
    }
}
