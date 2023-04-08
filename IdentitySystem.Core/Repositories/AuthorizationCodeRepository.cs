using AutoMapper;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class AuthorizationCodeRepository : IAuthorizationCodeRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public AuthorizationCodeRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<AuthorizationCodeDTO> CreateAuthorizationCode(AuthorizationCodeDTO authorizationCode)
    {
        var entity = mapper.Map<AuthorizationCode>(authorizationCode);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return mapper.Map<AuthorizationCodeDTO>(entity);
    }

    public async Task<string> GetUserId(int authorizationId)
    {
        return await context.AuthorizationCodes.AsNoTracking()
                                               .Where(x => x.Id == authorizationId)
                                               .Select(x => x.UserId)
                                               .FirstOrDefaultAsync();
    }

    public async Task<int> GetAuthorizationId(Guid appId, string code)
    {
        return await context.AuthorizationCodes.AsNoTracking()
                                               .Where(x => x.AppId == appId && x.Code == code && x.Expire >= DateTime.UtcNow)
                                               .Select(x => x.Id)
                                               .FirstOrDefaultAsync();
    }

    public async Task DeleteAuthorizationCode(int id)
    {
        await context.AuthorizationCodes.AsNoTracking()
                                        .Where(x => x.Id == id)
                                        .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckAuthorizationCodeExists(Guid appId, string code)
    {
        return await context.AuthorizationCodes.AsNoTracking()
                                               .AnyAsync(x => x.AppId == appId && x.Code == code && x.Expire >= DateTime.UtcNow);
    }
}
