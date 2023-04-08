using AutoMapper;
using EFCore.BulkExtensions;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class RefreshTokenScopeRepository : IRefreshTokenScopeRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public RefreshTokenScopeRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task CreateRefreshTokenScopes(List<RefreshTokenScopeDTO> refreshTokenScopes)
    {
        var entities = mapper.Map<List<RefreshTokenScope>>(refreshTokenScopes);
        entities.ForEach(x => x.Id = 0);

        await context.BulkInsertAsync(entities);
    }

    public async Task<List<string>> GetRefreshTokenScopes(int refreshTokenId)
    {
        return await context.RefreshTokenScopes.AsNoTracking()
                                               .Where(x => x.RefreshTokenId == refreshTokenId)
                                               .Select(x => x.Scope)
                                               .ToListAsync();
    }

    public async Task DeleteRefreshTokenScopes(int refreshTokenId)
    {
        await context.RefreshTokenScopes.AsNoTracking()
                                        .Where(x => x.RefreshTokenId == refreshTokenId)
                                        .ExecuteDeleteAsync();
    }
}
