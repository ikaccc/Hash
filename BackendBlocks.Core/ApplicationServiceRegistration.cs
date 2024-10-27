using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using BackendBlocks.Core.Behaviors;
using BackendBlocks.Core.Mappings;

namespace BackendBlocks.Core;

public static class ApplicationServiceRegistration
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton(sp => MapperDecorator.CreateMapper());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}
