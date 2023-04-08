using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class RefreshTokenScopeRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateRefreshTokenScopes_Success([Frozen] IdentityContext context, List<RefreshTokenScopeDTO> scopes, RefreshTokenScopeRepository sut)
    {
        await sut.CreateRefreshTokenScopes(scopes);
        var db = await context.RefreshTokenScopes.AsNoTracking().ToListAsync();

        Assert.Equal(scopes.Count, db.Count);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRefreshTokenScopes_WrongId([Frozen] IdentityContext context, RefreshTokenScope entity, RefreshTokenScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRefreshTokenScopes(entity.RefreshTokenId * 2);

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRefreshTokenScopes_Success([Frozen] IdentityContext context, RefreshTokenScope entity, RefreshTokenScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRefreshTokenScopes(entity.RefreshTokenId);

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRefreshTokenScopes_WrongId([Frozen] IdentityContext context, RefreshTokenScope entity, RefreshTokenScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRefreshTokenScopes(entity.RefreshTokenId * 2);
        var db = await context.RefreshTokenScopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRefreshTokenScopes_Success([Frozen] IdentityContext context, RefreshTokenScope entity, RefreshTokenScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRefreshTokenScopes(entity.RefreshTokenId);
        var db = await context.RefreshTokenScopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }
}
