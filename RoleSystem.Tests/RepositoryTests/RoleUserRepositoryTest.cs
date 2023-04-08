using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Repositories;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.RepositoryTests;

public class RoleUserRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRoleUserIds_NotFound([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRoleUserIds(entity.RoleId * 2);

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRoleUserIds_Success([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRoleUserIds(entity.RoleId);

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRoleIds_NotFound([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRoleIds(entity.UserId + "asd");

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRoleIds_Success([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRoleIds(entity.UserId);

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateRoleUser_Success([Frozen] RoleContext context, RoleUserDTO dto, RoleUserRepository sut)
    {
        await sut.CreateRoleUser(dto);
        var db = await context.RoleUsers.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.Equal(dto.RoleId, db.RoleId);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRoleUser_NotFound([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRoleUser(entity.RoleId * 2, entity.UserId);
        var db = await context.RoleUsers.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRoleUser_Success([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRoleUser(entity.RoleId, entity.UserId);
        var db = await context.RoleUsers.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRoleUsers_NotFound([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRoleUsers(entity.RoleId * 2);
        var db = await context.RoleUsers.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRoleUsers_Success([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRoleUsers(entity.RoleId);
        var db = await context.RoleUsers.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckRoleHasUser_NotFound([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckRoleHasUser(entity.RoleId * 2, entity.UserId);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckRoleHasUser_Success([Frozen] RoleContext context, RoleUser entity, RoleUserRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckRoleHasUser(entity.RoleId, entity.UserId);

        Assert.True(result);
    }
}
