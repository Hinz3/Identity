using AutoFixture.Xunit2;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using IdentitySystem.Tests.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Tests.RepositoryTests;

public class RefreshTokenRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRefreshTokenId_WrongCode([Frozen] IdentityContext context, RefreshToken entity, RefreshTokenRepository sut)
    {
        entity.Expire = DateTime.UtcNow.AddMinutes(5);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRefreshTokenId(entity.Token + "sad");

        Assert.Equal(0, result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRefreshTokenId_Expired([Frozen] IdentityContext context, RefreshToken entity, RefreshTokenRepository sut)
    {
        entity.Expire = DateTime.UtcNow.AddMinutes(-5);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRefreshTokenId(entity.Token);

        Assert.Equal(0, result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRefreshTokenId_NotExpired([Frozen] IdentityContext context, RefreshToken entity, RefreshTokenRepository sut)
    {
        entity.Expire = DateTime.UtcNow.AddMinutes(5);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRefreshTokenId(entity.Token);

        Assert.Equal(entity.Id, result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetUserId_WrongId([Frozen] IdentityContext context, RefreshToken entity, RefreshTokenRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetUserId(entity.Id * 2);

        Assert.Null(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetUserId_Success([Frozen] IdentityContext context, RefreshToken entity, RefreshTokenRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetUserId(entity.Id);

        Assert.Equal(entity.UserId, result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateRefreshToken_Success([Frozen] IdentityContext context, RefreshTokenDTO dto, RefreshTokenRepository sut)
    {
        await sut.CreateRefreshToken(dto);
        var db = await context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRefreshToken_WrongId([Frozen] IdentityContext context, RefreshToken entity, RefreshTokenRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRefreshToken(entity.Id * 2);
        var db = await context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRefreshToken_Success([Frozen] IdentityContext context, RefreshToken entity, RefreshTokenRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRefreshToken(entity.Id);
        var db = await context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }
}
