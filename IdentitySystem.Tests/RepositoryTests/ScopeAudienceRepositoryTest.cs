using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentitySystem.Tests.RepositoryTests;

public class ScopeAudienceRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAudiencesByScopes_WrongScopeName([Frozen] IdentityContext context, Scope scope, Audience audience, ScopeAudience scopeAudience,
        ScopeAudienceRepository sut)
    {
        await context.AddAsync(scope);
        await context.AddAsync(audience);
        await context.SaveChangesAsync();

        scopeAudience.ScopeId = scope.Id;
        scopeAudience.AudienceId = audience.Id;

        await context.AddAsync(scopeAudience);
        await context.SaveChangesAsync();

        var result = await sut.GetAudiencesByScopes(new List<string> { scope.Name + "Test" });

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAudiencesByScopes_ScopeNotActive([Frozen] IdentityContext context, Scope scope, Audience audience, ScopeAudience scopeAudience,
        ScopeAudienceRepository sut)
    {
        scope.IsActive = false;

        await context.AddAsync(scope);
        await context.AddAsync(audience);
        await context.SaveChangesAsync();

        scopeAudience.ScopeId = scope.Id;
        scopeAudience.AudienceId = audience.Id;

        await context.AddAsync(scopeAudience);
        await context.SaveChangesAsync();

        var result = await sut.GetAudiencesByScopes(new List<string> { scope.Name });

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAudiencesByScopes_Success([Frozen] IdentityContext context, Scope scope, Audience audience, ScopeAudience scopeAudience,
        ScopeAudienceRepository sut)
    {
        scope.IsActive = true;

        await context.AddAsync(scope);
        await context.AddAsync(audience);
        await context.SaveChangesAsync();

        scopeAudience.ScopeId = scope.Id;
        scopeAudience.AudienceId = audience.Id;

        await context.AddAsync(scopeAudience);
        await context.SaveChangesAsync();

        var result = await sut.GetAudiencesByScopes(new List<string> { scope.Name });

        Assert.Single(result);
        Assert.Equal(audience.Name, result.First());
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateScopeAudience_Success([Frozen] IdentityContext context, ScopeAudienceDTO dto, ScopeAudienceRepository sut)
    {
        await sut.CreateScopeAudience(dto);
        var db = await context.ScopeAudiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteScopeAudience_NotFound([Frozen] IdentityContext context, ScopeAudience entity, ScopeAudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteScopeAudience(entity.ScopeId, entity.AudienceId * 2);
        var db = await context.ScopeAudiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteScopeAudience_Success([Frozen] IdentityContext context, ScopeAudience entity, ScopeAudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteScopeAudience(entity.ScopeId, entity.AudienceId);
        var db = await context.ScopeAudiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckScopeHasAudience_NotFound([Frozen] IdentityContext context, ScopeAudience entity, ScopeAudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckScopeHasAudience(entity.ScopeId, entity.AudienceId * 2);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckScopeHasAudience_Success([Frozen] IdentityContext context, ScopeAudience entity, ScopeAudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckScopeHasAudience(entity.ScopeId, entity.AudienceId);

        Assert.True(result);
    }
}
