using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.Core.Repositories;

public class RoleFunctionRepository : IRoleFunctionRepository
{
    private readonly RoleContext context;
    private readonly IMapper mapper;

    public RoleFunctionRepository(RoleContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Dictionary<int, List<int>>> GetAllRoleFunctions()
    {
        return await context.RoleFunctions.AsNoTracking()
                                          .Select(x => new { x.RoleId, x.FunctionId })
                                          .GroupBy(x => x.RoleId)
                                          .ToDictionaryAsync(x => x.Key, x => x.Select(x => x.FunctionId).ToList());
    }

    public async Task CreateRoleFunction(RoleFunctionDTO roleFunction)
    {
        var entity = mapper.Map<RoleFunction>(roleFunction);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteRoleFunction(int roleId, int functionId)
    {
        await context.RoleFunctions.AsNoTracking()
                                   .Where(x => x.RoleId == roleId && x.FunctionId == functionId)
                                   .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckRoleHasFunction(int roleId, int functionId)
    {
        return await context.RoleFunctions.AsNoTracking().AnyAsync(x => x.RoleId == roleId && x.FunctionId == functionId);
    }
}
