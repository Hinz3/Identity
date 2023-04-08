using Common.DTOs;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Contracts.Enums;
using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Interfaces.Services;
using IdentitySystem.Core.Models;
using System.Security.Cryptography;
using System.Web;

namespace IdentitySystem.Core.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationCodeRepository authorizationCodeRepository;
    private readonly IAuthorizationScopeRepository authorizationScopeRepository;
    private readonly ISignInRepository signInRepository;
    private readonly IValidator<LoginParameters> validator;
    private readonly IValidator<TokenParameters> tokenValidator;
    private readonly ITokenService tokenService;
    private readonly IEnumerable<ITokenGrantType> grantTypes;

    public AuthorizationService(IAuthorizationCodeRepository authorizationCodeRepository, IAuthorizationScopeRepository authorizationScopeRepository,
        ISignInRepository signInRepository, IValidator<LoginParameters> loginValidator, IValidator<TokenParameters> tokenValidator, ITokenService tokenService,
        IEnumerable<ITokenGrantType> grantTypes)
    {
        this.authorizationCodeRepository = authorizationCodeRepository;
        this.authorizationScopeRepository = authorizationScopeRepository;
        this.signInRepository = signInRepository;
        this.validator = loginValidator;
        this.tokenValidator = tokenValidator;
        this.tokenService = tokenService;
        this.grantTypes = grantTypes;
    }

    public async Task<ResponseDTO<string>> GenerateAuthorizationCode(string username, LoginParameters loginParameters)
    {
        var errors = await validator.Validate(loginParameters);
        if (errors.Any())
        {
            return new ResponseDTO<string>(errors);
        }

        var user = await signInRepository.GetIdentityUserByEmail(username);
        if (user == null)
        {
            return new ResponseDTO<string>(new ErrorDTO((int)ErrorCode.INVALID_REQUEST, "Invalid request"));
        }

        var appId = Guid.Parse(loginParameters.ClientId);

        var hashedCode = SHA512.HashData(Guid.NewGuid().ToByteArray());
        var code = Convert.ToBase64String(hashedCode);
        var authorizationCode = new AuthorizationCodeDTO
        {
            Id = 0,
            AppId = appId,
            Code = code,
            Expire = DateTime.UtcNow.AddMinutes(5),
            UserId = user.Id
        };

        authorizationCode = await authorizationCodeRepository.CreateAuthorizationCode(authorizationCode);

        var scopes = new List<AuthorizationScopeDTO>();
        foreach (var scope in loginParameters.Scopes)
        {
            scopes.Add(new AuthorizationScopeDTO
            {
                AuthorizationId = authorizationCode.Id,
                Scope = scope
            });
        }

        if (scopes.Any())
        {
            await authorizationScopeRepository.CreateAuthorizationScopes(scopes);
        }

        var uriBuilder = new UriBuilder(loginParameters.RedirectUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["code"] = code;

        if (!string.IsNullOrEmpty(loginParameters.State))
        {
            query["state"] = loginParameters.State;
        }

        uriBuilder.Query = query.ToString();
        var redirectUrl = uriBuilder.ToString();

        return new ResponseDTO<string>(redirectUrl);
    }

    public async Task<ResponseDTO<TokenResponseDTO>> GenerateToken(TokenParameters tokenParameters)
    {
        var errors = await tokenValidator.Validate(tokenParameters);
        if (errors.Any())
        {
            return new ResponseDTO<TokenResponseDTO>(errors);
        }

        var clientId = Guid.Parse(tokenParameters.ClientId);

        var grantResponse = await grantTypes.Where(x => x.GrantType == tokenParameters.GrantType).First().GetGrantType(clientId, tokenParameters.Code);

        var token = await tokenService.GenerateToken(grantResponse.UserId, grantResponse.Scopes);
        if (token == null)
        {
            return new ResponseDTO<TokenResponseDTO>(new ErrorDTO((int)ErrorCode.INVALID_REQUEST, "Invalid request"));
        }

        return new ResponseDTO<TokenResponseDTO>(token);
    }
}
