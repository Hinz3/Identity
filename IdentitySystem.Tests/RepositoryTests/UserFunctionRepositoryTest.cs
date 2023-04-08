using AutoFixture.Xunit2;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class UserFunctionRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetUserFunctions_WrongUserId([Frozen] IdentityContext context, UserFunction entity, UserFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetUserFunctions(entity.UserId + "test");

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetUserFunctions_Success([Frozen] IdentityContext context, UserFunction entity, UserFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetUserFunctions(entity.UserId);

        Assert.Single(result);
        Assert.Equal(entity.FunctionId, result.First());
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateUserFunctions_Success([Frozen] IdentityContext context, UserFunctionRepository sut)
    {
        await sut.CreateUserFunctions("test", new List<int> { 1 });
        var db = await context.UserFunctions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.Equal(1, db.FunctionId);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteUserFunctions_WrongUserId([Frozen] IdentityContext context, UserFunction entity, UserFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteUserFunctions(entity.UserId + "test");
        var db = await context.UserFunctions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetUserFunctionDeleteUserFunctions_Success([Frozen] IdentityContext context, UserFunction entity, UserFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteUserFunctions(entity.UserId);
        var db = await context.UserFunctions.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }
}
