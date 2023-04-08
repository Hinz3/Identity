using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IAudienceRepository
{
    Task<List<AudienceDTO>> GetAudiences();
    Task<AudienceDTO> GetAudience(int id);
    Task<AudienceDTO> CreateAudience(AudienceDTO audience);
    Task UpdateAudience(AudienceDTO audience);
    Task DeleteAudience(int id);
    Task<bool> CheckAudienceExists(int id);
    Task<bool> CheckAudienceExists(string name);
}
