using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces.Services;

public interface ITokenService
{
    Task<TokenResponseDTO> GenerateToken(string userId, List<string> scopes);
}
