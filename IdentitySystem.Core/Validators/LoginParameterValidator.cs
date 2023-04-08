using Common.DTOs;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Contracts.Enums;
using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Models;

namespace IdentitySystem.Core.Validators;

public class LoginParameterValidator : IValidator<LoginParameters>
{
    private readonly IAppRepository appRepository;
    private readonly IAppUrlRepository appUrlRepository;
    private readonly IScopeRepository scopeRepository;

    public LoginParameterValidator(IAppRepository appRepository, IAppUrlRepository appUrlRepository, IScopeRepository scopeRepository)
    {
        this.appRepository = appRepository;
        this.appUrlRepository = appUrlRepository;
        this.scopeRepository = scopeRepository;
    }

    public async Task<List<ErrorDTO>> Validate(LoginParameters value)
    {
        var errors = new List<ErrorDTO>();

        if (!Guid.TryParse(value.ClientId, out var clientId))
        {
            errors.Add(new ErrorDTO((int)ErrorCode.INVALID_CLIENT_ID, "Invalid client id"));
            return errors;
        }

        var isActive = await appRepository.CheckAppIsActive(clientId);
        if (!isActive)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.INVALID_CLIENT_ID, "Invalid client id"));
        }

        var exists = await appUrlRepository.CheckUrlExists(clientId, value.RedirectUrl);
        if (string.IsNullOrEmpty(value.RedirectUrl) || !exists)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.INVALID_REDIRECT_URL, "Invalid redirect url"));
        }

        foreach (var scope in value.Scopes)
        {
            var scopeExists = await scopeRepository.CheckScopeIsActive(scope);
            if (!scopeExists)
            {
                errors.Add(new ErrorDTO((int)ErrorCode.INVALID_SCOPE, $"{scope} is invalid"));
            }
        }


        return errors;
    }
}
