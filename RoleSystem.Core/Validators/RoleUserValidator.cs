using Common.DTOs;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Repositories;

namespace RoleSystem.Core.Validators;

public class RoleUserValidator : IValidator<RoleUserDTO>
{
    private readonly IRoleRepository roleRepository;
    private readonly IRoleUserRepository roleUserRepository;

    public RoleUserValidator(IRoleRepository roleRepository, IRoleUserRepository roleUserRepository)
    {
        this.roleRepository = roleRepository;
        this.roleUserRepository = roleUserRepository;
    }

    public async Task<List<ErrorDTO>> Validate(RoleUserDTO value)
    {
        var errors = new List<ErrorDTO>();

        var roleExists = await roleRepository.CheckRoleExists(value.RoleId);
        if (!roleExists)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.ROLE_NOT_FOUND, "Role not found"));
        }

        var roleHasUsers = await roleUserRepository.CheckRoleHasUser(value.RoleId, value.UserId);
        if (roleHasUsers)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.ROLE_ALREADY_HAS_USER, "User is already in role"));
        }

        return errors;
    }
}
