using Common.DTOs;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Repositories;

namespace RoleSystem.Core.Validators;

public class RoleFunctionValidator : IValidator<RoleFunctionDTO>
{
    private readonly IRoleRepository roleRepository;
    private readonly IFunctionRepository functionRepository;
    private readonly IRoleFunctionRepository roleFunctionRepository;

    public RoleFunctionValidator(IRoleRepository roleRepository, IFunctionRepository functionRepository, IRoleFunctionRepository roleFunctionRepository)
    {
        this.roleRepository = roleRepository;
        this.functionRepository = functionRepository;
        this.roleFunctionRepository = roleFunctionRepository;
    }

    public async Task<List<ErrorDTO>> Validate(RoleFunctionDTO value)
    {
        var errors = new List<ErrorDTO>();

        var roleExists = await roleRepository.CheckRoleExists(value.RoleId);
        if (!roleExists)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.ROLE_NOT_FOUND, "Role not found"));
        }

        var functionExists = await functionRepository.CheckFunctionExists(value.FunctionId);
        if (!functionExists)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.FUNCTION_NOT_FOUND, "Function not found"));
        }

        var alreadyHasFunction = await roleFunctionRepository.CheckRoleHasFunction(value.RoleId, value.FunctionId);
        if (alreadyHasFunction)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.ROLE_ALREADY_HAS_FUNCTION, "Role already has function"));
        }

        return errors;
    }
}
