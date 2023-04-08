using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Models;
using System.Security.Authentication;

namespace IdentitySystem.Core.GrantTypes;

/// <summary>
/// Gets user id and scopes based on grant type authorization_code
/// </summary>
public class AuthorizationCodeGrantType : ITokenGrantType
{
    private readonly IAuthorizationCodeRepository authorizationCodeRepository;
    private readonly IAuthorizationScopeRepository authorizationScopeRepository;

    public string GrantType => "authorization_code";

    public AuthorizationCodeGrantType(IAuthorizationCodeRepository authorizationCodeRepository, IAuthorizationScopeRepository authorizationScopeRepository)
    {
        this.authorizationCodeRepository = authorizationCodeRepository;
        this.authorizationScopeRepository = authorizationScopeRepository;
    }

    public async Task<GrantTypeResponse> GetGrantType(Guid appId, string code)
    {
        var authorizationId = await authorizationCodeRepository.GetAuthorizationId(appId, code);
        if (authorizationId == 0)
        {
            throw new InvalidCredentialException();
        }

        var userId = await authorizationCodeRepository.GetUserId(authorizationId);
        if (userId == null)
        {
            throw new InvalidCredentialException();
        }

        var scopes = await authorizationScopeRepository.GetAuthorizationScopes(authorizationId);

        // Remove Authorization Code right after used
        await authorizationScopeRepository.DeleteAuthorizationScopes(authorizationId);
        await authorizationCodeRepository.DeleteAuthorizationCode(authorizationId);

        return new GrantTypeResponse
        {
            Id = authorizationId,
            UserId = userId,
            Scopes = scopes
        };
    }
}
