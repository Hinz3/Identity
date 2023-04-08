using Common.DTOs;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Contracts.Enums;
using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Models;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentitySystem.Core.Validators;

public class TokenParametersValidator : IValidator<TokenParameters>
{
    private readonly List<string> grantTypes;
    private readonly IAppRepository appRepository;

    public TokenParametersValidator(IAppRepository appRepository, IEnumerable<ITokenGrantType> grantTypes)
    {
        this.appRepository = appRepository;
        this.grantTypes = grantTypes.Select(x => x.GrantType).ToList();
    }

    public async Task<List<ErrorDTO>> Validate(TokenParameters value)
    {
        var errors = new List<ErrorDTO>();

        if (!Guid.TryParse(value.ClientId, out var clientId))
        {
            errors.Add(new ErrorDTO((int)ErrorCode.INVALID_CLIENT_ID, "Invalid client id"));
            return errors;
        }

        if (!grantTypes.Contains(value.GrantType))
        {
            errors.Add(new ErrorDTO((int)ErrorCode.INVALID_GRANT_TYPE, "Invalid grant type"));
        }

        var isActive = await appRepository.CheckAppIsActive(clientId);
        if (!isActive)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.INVALID_CLIENT_ID, "Invalid client id"));
        }

        var clientSecretExists = await appRepository.CheckClientSecrectExists(clientId, value.ClientSecret);
        if (!clientSecretExists)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.INVALID_CLIENT_ID, "Invalid client id"));
        }

        return errors;
    }
}
