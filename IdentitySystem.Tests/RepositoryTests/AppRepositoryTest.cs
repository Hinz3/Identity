using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class AppRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetApp_NotFound([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetApp(Guid.NewGuid());

        Assert.Null(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetApp_Success([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetApp(entity.Id);

        Assert.NotNull(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateApp_Success([Frozen] IdentityContext context, AppDTO app, AppRepository sut)
    {
        await sut.CreateApp(app);
        var db = await context.Apps.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.Equal(app.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateApp_NotFound([Frozen] IdentityContext context, AppDTO app, App entity, AppRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        app.Id = Guid.NewGuid();
        await sut.UpdateApp(app);
        var db = await context.Apps.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotEqual(app.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateApp_Success([Frozen] IdentityContext context, AppDTO app, App entity, AppRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        app.Id = entity.Id;
        await sut.UpdateApp(app);
        var db = await context.Apps.AsNoTracking().FirstOrDefaultAsync();

        Assert.Equal(app.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAppExists_NotFound([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAppExists(Guid.NewGuid());

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAppExists_Success([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAppExists(entity.Id);

        Assert.True(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAppIsActive_NotActive([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        entity.IsActive = false;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAppIsActive(entity.Id);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAppIsActive_Active([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        entity.IsActive = true;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAppIsActive(entity.Id);

        Assert.True(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckClientSecrectExists_NotActive([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        entity.IsActive = false;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckClientSecrectExists(entity.Id, entity.ClientSecret);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckClientSecrectExists_WrongSecret([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        entity.IsActive = true;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckClientSecrectExists(entity.Id, entity.ClientSecret + "asdsad");

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckClientSecrectExists_Success([Frozen] IdentityContext context, App entity, AppRepository sut)
    {
        entity.IsActive = true;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckClientSecrectExists(entity.Id, entity.ClientSecret);

        Assert.True(result);
    }
}
