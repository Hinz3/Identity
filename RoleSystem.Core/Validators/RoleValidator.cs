using Common.DTOs;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;

namespace RoleSystem.Core.Validators;

public class RoleValidator : IValidator<RoleDTO>
{
    public Task<List<ErrorDTO>> Validate(RoleDTO value)
    {
        var errors = new List<ErrorDTO>();

        if (string.IsNullOrEmpty(value.Name))
        {
            errors.Add(new ErrorDTO((int)ErrorCode.ROLE_NAME_REQUIRED, "Name is required"));
        }

        return Task.FromResult(errors);
    }
}
