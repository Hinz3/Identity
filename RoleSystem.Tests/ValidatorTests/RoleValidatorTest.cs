using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Validators;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.ValidatorTests;

public class RoleValidatorTest
{
    [Theory]
    [AutoMoqData]
    public async void Validate_NameNotSet(RoleDTO role, RoleValidator sut)
    {
        role.Name = null;

        var result = await sut.Validate(role);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.ROLE_NAME_REQUIRED);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_NameIsSet(RoleDTO role, RoleValidator sut)
    {
        role.Name = "test";

        var result = await sut.Validate(role);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.ROLE_NAME_REQUIRED);
    }
}
