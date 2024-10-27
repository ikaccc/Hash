using BackendBlocks.Core.Contracts.Persistence;
using BackendBlocks.Infrastructure.Persistence;
using BackendBlocks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackendBlocks.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<HashDBContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("ConnectionString"));
        });

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
    }

    public static void AddInfrastructureServicesSingleton(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<HashDBContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("ConnectionString"));
        }, ServiceLifetime.Singleton);

        services.AddSingleton(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
    }
}
