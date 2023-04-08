using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Configurations;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Providers;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Core.Mappings;
using RoleSystem.Core.Policies;
using RoleSystem.Core.Providers;
using RoleSystem.Core.Repositories;
using RoleSystem.Core.Services;
using RoleSystem.Core.Validators;

namespace RoleSystem.Core;

public static class ServiceCollectionMethods
{
    public static IServiceCollection UseCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseRepositories();
        services.UseServices();
        services.UseValidators();
        services.UsePolicies();
        services.UseProviders();
        services.UseConfigurations(configuration);

        services.AddAutoMapper(typeof(EventMappings), typeof(EntityMappings));

        return services;
    }

    public static IServiceCollection UseConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("JWT").Get<JWTConfiguration>());

        return services;
    }

    public static IServiceCollection UseRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFunctionRepository, FunctionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleFunctionRepository, RoleFunctionRepository>();
        services.AddScoped<IRoleUserRepository, RoleUserRepository>();

        return services;
    }

    public static IServiceCollection UseServices(this IServiceCollection services)
    {
        services.AddScoped<IFunctionService, FunctionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRoleFunctionService, RoleFunctionService>();
        services.AddScoped<IRoleUserService, RoleUserService>();
        services.AddScoped<IRoleEventService, RoleEventService>();

        return services;
    }

    public static IServiceCollection UseValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<FunctionDTO>, FunctionValidator>();
        services.AddScoped<IValidator<RoleDTO>, RoleValidator>();
        services.AddScoped<IValidator<RoleFunctionDTO>, RoleFunctionValidator>();
        services.AddScoped<IValidator<RoleUserDTO>, RoleUserValidator>();

        return services;
    }

    public static IServiceCollection UsePolicies(this IServiceCollection services)
    {
        services.AddScoped<IPolicy<FunctionDTO>, FunctionPolicy>();
        services.AddScoped<IPolicy<RoleDTO>, RolePolicy>();

        return services;
    }

    public static IServiceCollection UseProviders(this IServiceCollection services)
    {
        services.AddScoped<IFunctionProvider, FunctionProvider>();

        return services;
    }
}
