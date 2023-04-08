using EasyNetQDI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoleSystem.EventHandlers.RoleEvents;
using System.Reflection;

namespace RoleSystem.EventHandlers;

public static class ServiceCollectionMethods
{
    public static IServiceCollection UseEventHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseRabbit(configuration);
        services.AddSubscriber<RoleDeletedEventHandler>();
        services.AddSubscriber<RoleFunctionAddedEventHandler>();
        services.AddSubscriber<RoleFunctionRemovedEventHandler>();
        services.AddSubscriber<RoleUserAddedEventHandler>();
        services.AddSubscriber<RoleUserRemovedEventHandler>();

        return services;
    }

    public static IApplicationBuilder UseSubscriber(this IApplicationBuilder app)
    {
        app.UseSubscribe(Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly());

        return app;
    }
}
