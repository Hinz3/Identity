using AutoMapper;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class ScopeRepository : IScopeRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public ScopeRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<ScopeDTO>> GetScopes(bool showOnlyActive)
    {
        var query = context.Scopes.AsNoTracking();
        
        if (showOnlyActive)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query.AsNoTracking()
                          .Select(x => mapper.Map<ScopeDTO>(x))
                          .ToListAsync();
    }

    public async Task<ScopeDTO> GetScope(int id)
    {
        return await context.Scopes.AsNoTracking()
                                   .Where(x => x.Id == id)
                                   .Select(x => mapper.Map<ScopeDTO>(x))
                                   .FirstOrDefaultAsync();
    }

    public async Task<ScopeDTO> CreateScope(ScopeDTO scope)
    {
        var entity = mapper.Map<Scope>(scope);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return mapper.Map<ScopeDTO>(entity);
    }

    public async Task UpdateScope(ScopeDTO scope)
    {
        await context.Scopes.AsNoTracking()
                            .Where(x => x.Id == scope.Id)
                            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, scope.Name)
                                                      .SetProperty(p => p.Description, scope.Description)
                                                      .SetProperty(p => p.IsActive, scope.IsActive)
                                                      .SetProperty(p => p.LastEdit, scope.LastEdit)
                                                      .SetProperty(p => p.LastEditUser, scope.LastEditUser));
    }

    public async Task DeleteScope(int id)
    {
        await context.Scopes.AsNoTracking()
                            .Where(x => x.Id == id)
                            .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckScopeExists(int id)
    {
        return await context.Scopes.AsNoTracking().AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckScopeIsActive(string name)
    {
        return await context.Scopes.AsNoTracking().AnyAsync(x => x.Name == name && x.IsActive);
    }
}
