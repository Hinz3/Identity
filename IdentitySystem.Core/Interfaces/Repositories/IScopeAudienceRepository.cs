using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IScopeAudienceRepository
{
    Task<List<string>> GetAudiencesByScopes(List<string> scopes);
    Task CreateScopeAudience(ScopeAudienceDTO scopeAudience);
    Task DeleteScopeAudience(int scopeId, int audienceId);
    Task<bool> CheckScopeHasAudience(int scopeId, int audienceId);
}
