using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoleSystem.DataContext.DataContexts;

namespace RoleSystem.DataContext;

public static class ServiceCollectionMethods
{
    public static IServiceCollection UseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RoleContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
