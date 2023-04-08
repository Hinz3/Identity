using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.Core.Repositories;

public class FunctionRepository : IFunctionRepository
{
    private readonly RoleContext context;
    private readonly IMapper mapper;

    public FunctionRepository(RoleContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<FunctionDTO>> GetFunctions()
    {
        return await context.Functions.AsNoTracking()
                                      .Select(x => mapper.Map<FunctionDTO>(x))
                                      .ToListAsync();
    }

    public async Task<List<FunctionDTO>> GetRoleFunctions(int roleId)
    {
        var query = from roleFunction in context.RoleFunctions.AsNoTracking()
                    join function in context.Functions.AsNoTracking() on roleFunction.FunctionId equals function.Id
                    where roleFunction.RoleId == roleId
                    select new FunctionDTO
                    {
                        Id = function.Id,
                        ParentFunctionId = function.ParentFunctionId,
                        Name = function.Name,
                        Description = function.Description,
                        Created = function.Created,
                        CreatedUser = function.CreatedUser,
                        LastEdit = function.LastEdit,
                        LastEditUser = function.LastEditUser
                    };

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<FunctionDTO> GetFunction(int id)
    {
        return await context.Functions.AsNoTracking()
                                      .Where(x => x.Id == id)
                                      .Select(x => mapper.Map<FunctionDTO>(x))
                                      .FirstOrDefaultAsync();
    }

    public async Task<FunctionDTO> CreateFunction(FunctionDTO function)
    {
        var entity = mapper.Map<Function>(function);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return mapper.Map<FunctionDTO>(entity);
    }

    public async Task UpdateFunction(FunctionDTO function)
    {
        await context.Functions.AsNoTracking()
                               .Where(x => x.Id == function.Id)
                               .ExecuteUpdateAsync(x => x.SetProperty(p => p.ParentFunctionId, function.ParentFunctionId)
                                                         .SetProperty(p => p.Name, function.Name)
                                                         .SetProperty(p => p.Description, function.Description)
                                                         .SetProperty(p => p.LastEdit, function.LastEdit)
                                                         .SetProperty(p => p.LastEditUser, function.LastEditUser));
    }

    public async Task DeleteFunction(int id)
    {
        await context.Functions.AsNoTracking()
                               .Where(x => x.Id == id)
                               .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckFunctionExists(int id)
    {
        return await context.Functions.AsNoTracking().AnyAsync(x => x.Id == id);
    }
}
