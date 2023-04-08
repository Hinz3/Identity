using AutoFixture.Xunit2;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentitySystem.Tests.RepositoryTests;

public class AppUrlRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckUrlExists_WrongAppId([Frozen] IdentityContext context, AppUrl entity, AppUrlRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckUrlExists(Guid.NewGuid(), entity.Url);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckUrlExists_WrongUrl([Frozen] IdentityContext context, AppUrl entity, AppUrlRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckUrlExists(entity.AppId, entity.Url + "sds");

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckUrlExists_Success([Frozen] IdentityContext context, AppUrl entity, AppUrlRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckUrlExists(entity.AppId, entity.Url);

        Assert.True(result);
    }
}
