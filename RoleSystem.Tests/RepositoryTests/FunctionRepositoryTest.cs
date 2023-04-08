using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Repositories;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.RepositoryTests;

public class FunctionRepositoryTest
{
    [Theory]
    [AutoMoqSQLiteData]
    public async void GetFunctions_Success([Frozen] RoleContext context, Function entity, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetFunctions();

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRoleFunctions_WrongRoleId([Frozen] RoleContext context, Function entity, RoleFunction roleFunction, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        roleFunction.FunctionId = entity.Id;

        await context.AddAsync(roleFunction);
        await context.SaveChangesAsync();

        var result = await sut.GetRoleFunctions(roleFunction.RoleId * 2);

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetRoleFunctions_Success([Frozen] RoleContext context, Function entity, RoleFunction roleFunction, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        roleFunction.FunctionId = entity.Id;

        await context.AddAsync(roleFunction);
        await context.SaveChangesAsync();

        var result = await sut.GetRoleFunctions(roleFunction.RoleId);

        Assert.Single(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetFunction_NotFound([Frozen] RoleContext context, Function entity, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetFunction(entity.Id * 2);

        Assert.Null(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void GetFunction_Success([Frozen] RoleContext context, Function entity, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.GetFunction(entity.Id);

        Assert.NotNull(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CreateFunction_Success([Frozen] RoleContext context, FunctionDTO dto, FunctionRepository sut)
    {
        var result = await sut.CreateFunction(dto);
        var db = await context.Functions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(db);
        Assert.Equal(result.Id, db.Id);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateFunction_NotFound([Frozen] RoleContext context, Function entity, FunctionDTO dto, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id * 2;
        await sut.UpdateFunction(dto);
        var db = await context.Functions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.NotEqual(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void UpdateFunction_Success([Frozen] RoleContext context, Function entity, FunctionDTO dto, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        dto.Id = entity.Id;
        await sut.UpdateFunction(dto);
        var db = await context.Functions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.Equal(dto.Name, db.Name);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteFunction_NotFound([Frozen] RoleContext context, Function entity, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteFunction(entity.Id * 2);
        var db = await context.Functions.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void DeleteFunction_Success([Frozen] RoleContext context, Function entity, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await sut.DeleteFunction(entity.Id);
        var db = await context.Functions.AsNoTracking().FirstOrDefaultAsync();

        Assert.Null(db);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckFunctionExists_NotFound([Frozen] RoleContext context, Function entity, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckFunctionExists(entity.Id * 2);

        Assert.False(result);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void CheckFunctionExists_Success([Frozen] RoleContext context, Function entity, FunctionRepository sut)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await sut.CheckFunctionExists(entity.Id);

        Assert.True(result);
    }
}
