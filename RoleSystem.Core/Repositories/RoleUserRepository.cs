using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.Core.Repositories;

public class RoleUserRepository : IRoleUserRepository
{
    private readonly RoleContext context;
    private readonly IMapper mapper;

    public RoleUserRepository(RoleContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<string>> GetRoleUserIds(int roleId)
    {
        return await context.RoleUsers.AsNoTracking()
                                      .Where(x => x.RoleId == roleId)
                                      .Select(x => x.UserId)
                                      .ToListAsync();
    }

    public async Task<List<int>> GetRoleIds(string userId)
    {
        return await context.RoleUsers.AsNoTracking()
                                      .Where(x => x.UserId == userId)
                                      .Select(x => x.RoleId)
                                      .ToListAsync();
    }

    public async Task CreateRoleUser(RoleUserDTO roleUser)
    {
        var entity = mapper.Map<RoleUser>(roleUser);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteRoleUser(int roleId, string userId)
    {
        await context.RoleUsers.AsNoTracking()
                               .Where(x => x.RoleId == roleId && x.UserId == userId)
                               .ExecuteDeleteAsync();
    }

    public async Task DeleteRoleUsers(int roleId)
    {
        await context.RoleUsers.AsNoTracking()
                               .Where(x => x.RoleId == roleId)
                               .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckRoleHasUser(int roleId, string userId)
    {
        return await context.RoleUsers.AsNoTracking().AnyAsync(x => x.RoleId == roleId && x.UserId == userId);
    }
}
