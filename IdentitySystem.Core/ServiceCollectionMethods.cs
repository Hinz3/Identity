using IdentitySystem.Core.Configurations;
using IdentitySystem.Core.GrantTypes;
using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Interfaces.Services;
using IdentitySystem.Core.Mappings;
using IdentitySystem.Core.Models;
using IdentitySystem.Core.Repositories;
using IdentitySystem.Core.Services;
using IdentitySystem.Core.Validators;
using IdentitySystem.DataContext.DataContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentitySystem.Core;

public static class ServiceCollectionMethods
{
    public static IServiceCollection UseCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityContext>();

        services.UseRepositories();
        services.UseServices();
        services.UseValidators();
        services.UsePolicies();
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
        services.AddScoped<IAppRepository, AppRepository>();
        services.AddScoped<IAppUrlRepository, AppUrlRepository>();
        services.AddScoped<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
        services.AddScoped<IAuthorizationScopeRepository, AuthorizationScopeRepository>();
        services.AddScoped<IScopeRepository, ScopeRepository>();
        services.AddScoped<ISignInRepository, SignInRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenScopeRepository, RefreshTokenScopeRepository>();
        services.AddScoped<IUserFunctionRepository, UserFunctionRepository>();
        services.AddScoped<IAudienceRepository, AudienceRepository>();
        services.AddScoped<IScopeAudienceRepository, ScopeAudienceRepository>();

        return services;
    }

    public static IServiceCollection UseServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserFunctionEventService, UserFunctionEventService>();

        return services;
    }

    public static IServiceCollection UseValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoginParameters>, LoginParameterValidator>();
        services.AddScoped<IValidator<TokenParameters>, TokenParametersValidator>();

        return services;
    }

    public static IServiceCollection UsePolicies(this IServiceCollection services)
    {
        services.AddScoped<ITokenGrantType, AuthorizationCodeGrantType>();
        services.AddScoped<ITokenGrantType, RefreshCodeGrantType>();

        return services;
    }
}
