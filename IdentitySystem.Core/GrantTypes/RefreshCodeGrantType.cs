using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Models;
using System.Security.Authentication;

namespace IdentitySystem.Core.GrantTypes;

/// <summary>
/// Get user id and scopes from grant type refresh_code
/// </summary>
public class RefreshCodeGrantType : ITokenGrantType
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IRefreshTokenScopeRepository refreshTokenScopeRepository;

    public string GrantType => "refresh_code";

    public RefreshCodeGrantType(IRefreshTokenRepository refreshTokenRepository, IRefreshTokenScopeRepository refreshTokenScopeRepository)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.refreshTokenScopeRepository = refreshTokenScopeRepository;
    }

    public async Task<GrantTypeResponse> GetGrantType(Guid appId, string code)
    {
        var refreshTokenId = await refreshTokenRepository.GetRefreshTokenId(code);
        if (refreshTokenId == 0)
        {
            // Refresh token does not exists or is expired
            throw new InvalidCredentialException();
        }

        var userId = await refreshTokenRepository.GetUserId(refreshTokenId);
        var scopes = await refreshTokenScopeRepository.GetRefreshTokenScopes(refreshTokenId);

        // Remove refresh tokens right after they have been used
        await refreshTokenRepository.DeleteRefreshToken(refreshTokenId);
        await refreshTokenScopeRepository.DeleteRefreshTokenScopes(refreshTokenId);

        return new GrantTypeResponse
        {
            Id = refreshTokenId,
            UserId = userId,
            Scopes = scopes
        };
    }
}
