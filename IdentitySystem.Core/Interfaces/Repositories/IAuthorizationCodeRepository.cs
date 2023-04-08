using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Repositories
{
    public interface IAuthorizationCodeRepository
    {
        Task<AuthorizationCodeDTO> CreateAuthorizationCode(AuthorizationCodeDTO authorizationCode);
        Task<string> GetUserId(int authorizationId);
        Task<int> GetAuthorizationId(Guid appId, string code);
        Task DeleteAuthorizationCode(int id);
        Task<bool> CheckAuthorizationCodeExists(Guid appId, string code);
    }
}
