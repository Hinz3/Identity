using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IRefreshTokenScopeRepository
{
    Task CreateRefreshTokenScopes(List<RefreshTokenScopeDTO> refreshTokenScopes);
    Task<List<string>> GetRefreshTokenScopes(int refreshTokenId);
    Task DeleteRefreshTokenScopes(int refreshTokenId);
}
