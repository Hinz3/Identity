using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IScopeRepository
{
    Task<List<ScopeDTO>> GetScopes(bool showOnlyActive);
    Task<ScopeDTO> GetScope(int id);
    Task<ScopeDTO> CreateScope(ScopeDTO scope);
    Task UpdateScope(ScopeDTO scope);
    Task DeleteScope(int id);
    Task<bool> CheckScopeExists(int id);
    Task<bool> CheckScopeIsActive(string name);
}
