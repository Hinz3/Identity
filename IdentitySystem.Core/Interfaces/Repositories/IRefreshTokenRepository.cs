using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<int> GetRefreshTokenId(string refreshToken);
    Task<string> GetUserId(int refreshTokenId);
    Task<RefreshTokenDTO> CreateRefreshToken(RefreshTokenDTO refreshToken);
    Task DeleteRefreshToken(int refreshTokenId);
}
