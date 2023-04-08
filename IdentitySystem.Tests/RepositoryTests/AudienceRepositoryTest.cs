using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class AudienceRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAudiences_Success([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAudiences();

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAudience_NotFound([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAudience(entity.Id * 2);

        Assert.Null(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAudience_Success([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAudience(entity.Id);

        Assert.NotNull(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateAudience_Success([Frozen] IdentityContext context, AudienceDTO dto, AudienceRepository sut)
    {
        var result = await sut.CreateAudience(dto);
        var db = await context.Audiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateAudience_NotFound([Frozen] IdentityContext context, Audience entity, AudienceDTO dto, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id * 2;
        await sut.UpdateAudience(dto);
        var db = await context.Audiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotEqual(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateAudience_Success([Frozen] IdentityContext context, Audience entity, AudienceDTO dto, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id;
        await sut.UpdateAudience(dto);
        var db = await context.Audiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.Equal(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteAudience_NotFound([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteAudience(entity.Id * 2);
        var db = await context.Audiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteAudience_Success([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteAudience(entity.Id);
        var db = await context.Audiences.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAudienceExists_NotFound([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAudienceExists(entity.Id * 2);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAudienceExists_Success([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAudienceExists(entity.Id);

        Assert.True(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAudienceExists_ByName_WrongName([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAudienceExists(entity.Name + "sad");

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAudienceExists_ByName_Success([Frozen] IdentityContext context, Audience entity, AudienceRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAudienceExists(entity.Name);

        Assert.True(result);
    }
}
