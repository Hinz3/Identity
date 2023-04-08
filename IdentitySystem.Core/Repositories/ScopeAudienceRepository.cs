using AutoMapper;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class ScopeAudienceRepository : IScopeAudienceRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public ScopeAudienceRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<string>> GetAudiencesByScopes(List<string> scopes)
    {
        var query = from scopeAudience in context.ScopeAudiences.AsNoTracking()
                    join scope in context.Scopes.AsNoTracking() on scopeAudience.ScopeId equals scope.Id
                    join audience in context.Audiences.AsNoTracking() on scopeAudience.AudienceId equals audience.Id
                    where scopes.Contains(scope.Name) && scope.IsActive
                    select audience.Name;

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task CreateScopeAudience(ScopeAudienceDTO scopeAudience)
    {
        var entity = mapper.Map<ScopeAudience>(scopeAudience);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteScopeAudience(int scopeId, int audienceId)
    {
        await context.ScopeAudiences.AsNoTracking()
                                    .Where(x => x.ScopeId == scopeId && x.AudienceId == audienceId)
                                    .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckScopeHasAudience(int scopeId, int audienceId)
    {
        return await context.ScopeAudiences.AsNoTracking().AnyAsync(x => x.ScopeId == scopeId && x.AudienceId == audienceId);
    }
}
