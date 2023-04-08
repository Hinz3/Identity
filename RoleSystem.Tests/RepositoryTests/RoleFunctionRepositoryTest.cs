using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Repositories;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.RepositoryTests;

public class RoleFunctionRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetAllRoleFunctions_Success([Frozen] RoleContext context, RoleFunction entity, RoleFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetAllRoleFunctions();

        Assert.Single(result[entity.RoleId]);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateRoleFunction_Success([Frozen] RoleContext context, RoleFunctionDTO dto, RoleFunctionRepository sut)
    {
        await sut.CreateRoleFunction(dto);
        var db = await context.RoleFunctions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.Equal(dto.RoleId, db.RoleId);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRoleFunction_NotFound([Frozen] RoleContext context, RoleFunction entity, RoleFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRoleFunction(entity.RoleId, entity.FunctionId * 2);
        var db = await context.RoleFunctions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteRoleFunction_Success([Frozen] RoleContext context, RoleFunction entity, RoleFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteRoleFunction(entity.RoleId, entity.FunctionId);
        var db = await context.RoleFunctions.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckRoleHasFunction_NotFound([Frozen] RoleContext context, RoleFunction entity, RoleFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckRoleHasFunction(entity.RoleId, entity.FunctionId * 2);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckRoleHasFunction_Success([Frozen] RoleContext context, RoleFunction entity, RoleFunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckRoleHasFunction(entity.RoleId, entity.FunctionId);

        Assert.True(result);
    }
}
