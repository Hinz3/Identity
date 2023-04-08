using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class AuthorizationCodeRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateAuthorizationCode_Success([Frozen] IdentityContext context, AuthorizationCodeDTO dto, AuthorizationCodeRepository sut)
    {
        await sut.CreateAuthorizationCode(dto);
        var db = await context.AuthorizationCodes.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.Equal(dto.Code, db.Code);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetUserId_NotFound([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetUserId(entity.Id * 2);

        Assert.Null(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetUserId_Success([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetUserId(entity.Id);

        Assert.Equal(entity.UserId, result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAuthorizationId_Expired([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        entity.Expire = DateTime.UtcNow.AddMinutes(-1);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAuthorizationId(entity.AppId, entity.Code);

        Assert.Equal(0, result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAuthorizationId_NotExpired([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        entity.Expire = DateTime.UtcNow.AddMinutes(1);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAuthorizationId(entity.AppId, entity.Code);

        Assert.Equal(entity.Id, result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteAuthorizationCode_WrongId([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteAuthorizationCode(entity.Id * 2);
        var db = await context.AuthorizationCodes.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteAuthorizationCode_Success([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteAuthorizationCode(entity.Id);
        var db = await context.AuthorizationCodes.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAuthorizationCodeExists_Expired([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        entity.Expire = DateTime.UtcNow.AddMinutes(-1);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAuthorizationCodeExists(entity.AppId, entity.Code);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckAuthorizationCodeExists_NotExpired([Frozen] IdentityContext context, AuthorizationCode entity, AuthorizationCodeRepository sut)
    {
        entity.Expire = DateTime.UtcNow.AddMinutes(1);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckAuthorizationCodeExists(entity.AppId, entity.Code);

        Assert.True(result);
    }
}
