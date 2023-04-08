using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IAuthorizationScopeRepository
{
    Task CreateAuthorizationScopes(List<AuthorizationScopeDTO> authorizationScopes);
    Task<List<string>> GetAuthorizationScopes(int authorizationId);
    Task DeleteAuthorizationScopes(int authorizationId);
}
