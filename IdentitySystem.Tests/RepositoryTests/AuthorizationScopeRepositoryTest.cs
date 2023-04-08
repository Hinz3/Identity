using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class AuthorizationScopeRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateAuthorizationScopes_Success([Frozen] IdentityContext context, List<AuthorizationScopeDTO> scopes, AuthorizationScopeRepository sut)
    {
        await sut.CreateAuthorizationScopes(scopes);
        var db = await context.AuthorizationScopes.AsNoTracking().ToListAsync();

        Assert.Equal(scopes.Count, db.Count);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAuthorizationScopes_WrongId([Frozen] IdentityContext context, AuthorizationScope entity, AuthorizationScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAuthorizationScopes(entity.AuthorizationId * 2);

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAuthorizationScopes_Success([Frozen] IdentityContext context, AuthorizationScope entity, AuthorizationScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAuthorizationScopes(entity.AuthorizationId);

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteAuthorizationScopes_WrongId([Frozen] IdentityContext context, AuthorizationScope entity, AuthorizationScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteAuthorizationScopes(entity.AuthorizationId * 2);
        var db = await context.AuthorizationScopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteAuthorizationScopes_Success([Frozen] IdentityContext context, AuthorizationScope entity, AuthorizationScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteAuthorizationScopes(entity.AuthorizationId);
        var db = await context.AuthorizationScopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }
}
