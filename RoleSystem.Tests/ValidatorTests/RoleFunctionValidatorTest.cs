using AutoFixture.Xunit2;
using Moq;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Validators;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.ValidatorTests;

public class RoleFunctionValidatorTest
{
    [Theory]
    [AutoMoqData]
    public async void Validate_RoleNotFound([Frozen] Mock<IRoleRepository> repoMock, RoleFunctionDTO value, RoleFunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(false);

        var result = await sut.Validate(value);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RoleFound([Frozen] Mock<IRoleRepository> repoMock, RoleFunctionDTO value, RoleFunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(true);

        var result = await sut.Validate(value);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_FunctionNotFound([Frozen] Mock<IFunctionRepository> repoMock, RoleFunctionDTO value, RoleFunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(false);

        var result = await sut.Validate(value);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_FunctionFound([Frozen] Mock<IFunctionRepository> repoMock, RoleFunctionDTO value, RoleFunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(true);

        var result = await sut.Validate(value);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RoleAlreadyHasFunction([Frozen] Mock<IRoleFunctionRepository> repoMock, RoleFunctionDTO value, RoleFunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleHasFunction(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        var result = await sut.Validate(value);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.ROLE_ALREADY_HAS_FUNCTION);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RoleHasNotFunction([Frozen] Mock<IRoleFunctionRepository> repoMock, RoleFunctionDTO value, RoleFunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleHasFunction(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

        var result = await sut.Validate(value);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.ROLE_ALREADY_HAS_FUNCTION);
    }
}
