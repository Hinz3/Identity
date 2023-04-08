using AutoMapper;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public RefreshTokenRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<int> GetRefreshTokenId(string refreshToken)
    {
        return await context.RefreshTokens.AsNoTracking()
                                          .Where(x => x.Token == refreshToken && x.Expire >= DateTime.UtcNow)
                                          .Select(x => x.Id)
                                          .FirstOrDefaultAsync();
    }

    public async Task<string> GetUserId(int refreshTokenId)
    {
        return await context.RefreshTokens.AsNoTracking()
                                          .Where(x => x.Id == refreshTokenId)
                                          .Select(x => x.UserId)
                                          .FirstOrDefaultAsync();
    }

    public async Task<RefreshTokenDTO> CreateRefreshToken(RefreshTokenDTO refreshToken)
    {
        var entity = mapper.Map<RefreshToken>(refreshToken);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return mapper.Map<RefreshTokenDTO>(entity);
    }

    public async Task DeleteRefreshToken(int refreshTokenId)
    {
        await context.RefreshTokens.AsNoTracking()
                                   .Where(x => x.Id == refreshTokenId)
                                   .ExecuteDeleteAsync();
    }
}
