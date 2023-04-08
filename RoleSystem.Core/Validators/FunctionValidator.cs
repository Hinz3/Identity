using Common.DTOs;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Repositories;

namespace RoleSystem.Core.Validators;

public class FunctionValidator : IValidator<FunctionDTO>
{
    private readonly IFunctionRepository functionRepository;

    public FunctionValidator(IFunctionRepository functionRepository)
    {
        this.functionRepository = functionRepository;
    }

    public async Task<List<ErrorDTO>> Validate(FunctionDTO value)
    {
        var errors = new List<ErrorDTO>();

        if (string.IsNullOrEmpty(value.Name))
        {
            errors.Add(new ErrorDTO((int)ErrorCode.FUNCTION_NAME_REQUIRED, "Name is required"));
        }

        if (value.ParentFunctionId.HasValue && value.ParentFunctionId.Value == value.Id)
        {
            errors.Add(new ErrorDTO((int)ErrorCode.FUNCTION_PARENT_CANNOT_BE_SAME, "Parent Id cannot be the same as the function"));
        }

        if (value.ParentFunctionId.HasValue)
        {
            var exists = await functionRepository.CheckFunctionExists(value.ParentFunctionId.Value);
            if (!exists)
            {
                errors.Add(new ErrorDTO((int)ErrorCode.FUNCTION_NOT_FOUND, "Parent Function not found"));
            }
        }

        return errors;
    }
}
