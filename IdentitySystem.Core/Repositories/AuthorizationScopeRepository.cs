using AutoMapper;
using EFCore.BulkExtensions;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class AuthorizationScopeRepository : IAuthorizationScopeRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public AuthorizationScopeRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task CreateAuthorizationScopes(List<AuthorizationScopeDTO> authorizationScopes)
    {
        var entities = mapper.Map<List<AuthorizationScope>>(authorizationScopes);
        entities.ForEach(x => x.Id = 0);

        await context.BulkInsertAsync(entities);
    }

    public async Task<List<string>> GetAuthorizationScopes(int authorizationId)
    {
        return await context.AuthorizationScopes.AsNoTracking()
                                                .Where(x => x.AuthorizationId == authorizationId)
                                                .Select(x => x.Scope)
                                                .ToListAsync();
    }

    public async Task DeleteAuthorizationScopes(int authorizationId)
    {
        await context.AuthorizationScopes.AsNoTracking()
                                         .Where(x => x.AuthorizationId == authorizationId)
                                         .ExecuteDeleteAsync();
    }

}
