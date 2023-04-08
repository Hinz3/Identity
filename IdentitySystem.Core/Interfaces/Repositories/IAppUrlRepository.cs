namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IAppUrlRepository
{
    Task<bool> CheckUrlExists(Guid appId, string url);
}
