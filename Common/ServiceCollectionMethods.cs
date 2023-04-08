using Common.Implementations;
using Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public static class ServiceCollectionMethods
    {
        public static IServiceCollection UseCommon(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

            return services;
        }
    }
}
