using AutoMapper;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.Core.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleContext context;
    private readonly IMapper mapper;

    public RoleRepository(RoleContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<RoleDTO>> GetRoles()
    {
        return await context.Roles.AsNoTracking()
                                      .Select(x => mapper.Map<RoleDTO>(x))
                                      .ToListAsync();
    }

    public async Task<RoleDTO> GetRole(int id)
    {
        return await context.Roles.AsNoTracking()
                                      .Where(x => x.Id == id)
                                      .Select(x => mapper.Map<RoleDTO>(x))
                                      .FirstOrDefaultAsync();
    }

    public async Task<RoleDTO> CreateRole(RoleDTO Role)
    {
        var entity = mapper.Map<Role>(Role);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return mapper.Map<RoleDTO>(entity);
    }

    public async Task UpdateRole(RoleDTO Role)
    {
        await context.Roles.AsNoTracking()
                               .Where(x => x.Id == Role.Id)
                               .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, Role.Name)
                                                         .SetProperty(p => p.Description, Role.Description)
                                                         .SetProperty(p => p.LastEdit, Role.LastEdit)
                                                         .SetProperty(p => p.LastEditUser, Role.LastEditUser));
    }

    public async Task DeleteRole(int id)
    {
        await context.Roles.AsNoTracking()
                               .Where(x => x.Id == id)
                               .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckRoleExists(int id)
    {
        return await context.Roles.AsNoTracking().AnyAsync(x => x.Id == id);
    }
}