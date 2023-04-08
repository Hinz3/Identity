using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class ScopeRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetScopes_OnlyActive_NotActive([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        entity.IsActive = false;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetScopes(true);

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetScopes_OnlyActive_IsActive([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        entity.IsActive = true;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetScopes(true);

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetScopes_ShowAll([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        entity.IsActive = false;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetScopes(false);

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetScope_NotFound([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetScope(entity.Id * 2);

        Assert.Null(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetScope_Success([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetScope(entity.Id);

        Assert.NotNull(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateScope_Success([Frozen] IdentityContext context, ScopeDTO dto, ScopeRepository sut)
    {
        var result = await sut.CreateScope(dto);
        var db = await context.Scopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateScope_NotFound([Frozen] IdentityContext context, Scope entity, ScopeDTO dto, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id * 2;
        await sut.UpdateScope(dto);
        var db = await context.Scopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotEqual(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateScope_Success([Frozen] IdentityContext context, Scope entity, ScopeDTO dto, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id;
        await sut.UpdateScope(dto);
        var db = await context.Scopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.Equal(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteScope_NotFound([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteScope(entity.Id * 2);
        var db = await context.Scopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteScope_Success([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteScope(entity.Id);
        var db = await context.Scopes.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckScopeExists_NotFound([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckScopeExists(entity.Id * 2);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckScopeExists_Success([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckScopeExists(entity.Id);

        Assert.True(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckScopeIsActive_NotActive([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        entity.IsActive = false;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckScopeIsActive(entity.Name);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckScopeIsActive_WrongName([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        entity.IsActive = true;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckScopeIsActive(entity.Name + "sad");

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckScopeIsActive_Success([Frozen] IdentityContext context, Scope entity, ScopeRepository sut)
    {
        entity.IsActive = true;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckScopeIsActive(entity.Name);

        Assert.True(result);
    }
}
