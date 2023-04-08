using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IAppRepository
{
    Task<AppDTO> GetApp(Guid id);
    Task CreateApp(AppDTO app);
    Task UpdateApp(AppDTO app);
    Task DeleteApp(Guid id);
    Task<bool> CheckAppExists(Guid id);
    Task<bool> CheckAppIsActive(Guid id);
    Task<bool> CheckClientSecrectExists(Guid id, string clientSecret);
}
