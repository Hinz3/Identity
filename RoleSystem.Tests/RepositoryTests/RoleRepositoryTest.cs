using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Repositories;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;
using RoleSystem.Tests.Configuration;


namespace RoleSystem.Tests.RepositoryTests;

public class RoleRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRoles_Success([Frozen] RoleContext context, Role entity, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRoles();

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRole_NotFound([Frozen] RoleContext context, Role entity, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRole(entity.Id * 2);

        Assert.Null(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRole_Success([Frozen] RoleContext context, Role entity, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetRole(entity.Id);

        Assert.NotNull(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateRole_Success([Frozen] RoleContext context, RoleDTO dto, RoleRepository sut)
    {
        var result = await sut.CreateRole(dto);
        var db = await context.Roles.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(db);
        Assert.Equal(result.Id, db.Id);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateRole_NotFound([Frozen] RoleContext context, Role entity, RoleDTO dto, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id * 2;
        await sut.UpdateRole(dto);
        var db = await context.Roles.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.NotEqual(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateRole_Success([Frozen] RoleContext context, Role entity, RoleDTO dto, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id;
        await sut.UpdateRole(dto);
        var db = await context.Roles.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.Equal(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRole_NotFound([Frozen] RoleContext context, Role entity, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRole(entity.Id * 2);
        var db = await context.Roles.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRole_Success([Frozen] RoleContext context, Role entity, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRole(entity.Id);
        var db = await context.Roles.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckRoleExists_NotFound([Frozen] RoleContext context, Role entity, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckRoleExists(entity.Id * 2);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckRoleExists_Success([Frozen] RoleContext context, Role entity, RoleRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckRoleExists(entity.Id);

        Assert.True(result);
    }
}
