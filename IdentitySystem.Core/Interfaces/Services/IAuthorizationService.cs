using Common.DTOs;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Models;

namespace IdentitySystem.Core.Interfaces.Services;

public interface IAuthorizationService
{
    /// <summary>
    /// Generates redirect url
    /// </summary>
    /// <param name="username">Email of the user logged in</param>
    /// <param name="loginParameters">Loging parameters for the login app</param>
    /// <returns>Url to redirect</returns>
    Task<ResponseDTO<string>> GenerateAuthorizationCode(string username, LoginParameters loginParameters);

    /// <summary>
    /// Generates tokens from Authorization Code obtained in GenerateAuthorizationCode()
    /// </summary>
    /// <param name="tokenParameters"></param>
    /// <exception cref="InvalidCredentialException">Throws if code provide does not exists or expired</exception>
    /// <returns>Access token and refresh token</returns>
    Task<ResponseDTO<TokenResponseDTO>> GenerateToken(TokenParameters tokenParameters);
}
