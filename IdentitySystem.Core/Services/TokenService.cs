using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Configurations;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Interfaces.Services;
using IdentitySystem.DataContext.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace IdentitySystem.Core.Services;

public class TokenService : ITokenService
{
    private readonly JWTConfiguration configuration;
    private readonly ISignInRepository signInRepository;
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IRefreshTokenScopeRepository refreshTokenScopeRepository;
    private readonly IUserFunctionRepository userFunctionRepository;
    private readonly IScopeAudienceRepository scopeAudienceRepository;

    public TokenService(JWTConfiguration configuration, ISignInRepository signInRepository, IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenScopeRepository refreshTokenScopeRepository, IUserFunctionRepository userFunctionRepository, IScopeAudienceRepository scopeAudienceRepository)
    {
        this.configuration = configuration;
        this.signInRepository = signInRepository;
        this.refreshTokenRepository = refreshTokenRepository;
        this.refreshTokenScopeRepository = refreshTokenScopeRepository;
        this.userFunctionRepository = userFunctionRepository;
        this.scopeAudienceRepository = scopeAudienceRepository;
    }

    public async Task<TokenResponseDTO> GenerateToken(string userId, List<string> scopes)
    {
        var privateKey = Convert.FromBase64String(configuration.PrivateKey);

        using RSA rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKey, out _);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };

        var now = DateTime.Now;
        var expire = now.AddMinutes(15);
        var claims = new List<Claim>();

        var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

        var user = await signInRepository.GetIdentityUserById(userId);
        if (user == null)
        {
            return null;
        }

        claims.Add(new Claim("UserId", userId));
        claims.Add(new Claim("UserName", user.UserName));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var scopeClaims = await GetScopeClaims(userId, scopes);
        claims.AddRange(scopeClaims);

        var jwt = new JwtSecurityToken(
            //audience: configuration.Audience,
            issuer: configuration.Issuer,
            claims: claims.ToArray(),
            notBefore: now,
            expires: expire,
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = await GenerateRefreshToken(userId, scopes);
        
        return new TokenResponseDTO
        {
            AccessToken = token,
            TokenType = "Bearer",
            Expires = expire,
            RefreshToken = refreshToken,
            Scopes = scopes
        };
    }

    /// <summary>
    /// Get claims for user based on scopes
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="scopes"></param>
    /// <returns></returns>
    private async Task<List<Claim>> GetScopeClaims(string userId, List<string> scopes)
    {
        var claims = await GetUserFunctionClaims(userId);
        var audienceClaims = await GetAudienceClaims(scopes);

        claims.AddRange(audienceClaims);

        return claims;
    }

    /// <summary>
    /// Get claims based on user functions
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<List<Claim>> GetUserFunctionClaims(string userId)
    {
        var claims = new List<Claim>();

        var userFunctions = await userFunctionRepository.GetUserFunctions(userId);
        if (!userFunctions.Any())
        {
            return claims;
        }

        foreach (var userFunction in userFunctions)
        {
            if (claims.Any(x => x.Value == userFunction.ToString()))
            {
                // We do not want duplicated user functions
                continue;
            }

            claims.Add(new Claim("UF", userFunction.ToString()));
        }

        return claims;
    }

    private async Task<List<Claim>> GetAudienceClaims(List<string> scopes)
    {
        var claims = new List<Claim>();
        var audiences = await scopeAudienceRepository.GetAudiencesByScopes(scopes);

        foreach (var audience in audiences)
        {
            if (claims.Any(x => x.Value == audience))
            {
                // We do not want duplicated audiences
                continue;
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
        }

        return claims;
    }

    private async Task<string> GenerateRefreshToken(string userId, List<string> scopes)
    {
        var hashedCode = SHA512.HashData(Guid.NewGuid().ToByteArray());
        var code = Convert.ToBase64String(hashedCode);

        var refreshToken = new RefreshTokenDTO
        {
            Id = 0,
            Token = code,
            UserId = userId,
            Expire = DateTime.UtcNow.AddDays(30)
        };

        refreshToken = await refreshTokenRepository.CreateRefreshToken(refreshToken);

        if (scopes.Any())
        {
            var refreshScopes = new List<RefreshTokenScopeDTO>();
            scopes.ForEach(scope =>
            {
                refreshScopes.Add(new RefreshTokenScopeDTO
                {
                    Id = 0,
                    RefreshTokenId = refreshToken.Id,
                    Scope = scope
                });
            });

            await refreshTokenScopeRepository.CreateRefreshTokenScopes(refreshScopes);
        }

        return code;
    }
}
