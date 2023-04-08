using AutoFixture.Xunit2;
using Moq;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Validators;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.ValidatorTests;

public class RoleUserValidatorTest
{
    [Theory]
    [AutoMoqData]
    public async void Validate_RoleNotFound([Frozen] Mock<IRoleRepository> repoMock, RoleUserDTO value, RoleUserValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(false);

        var result = await sut.Validate(value);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RoleFound([Frozen] Mock<IRoleRepository> repoMock, RoleUserDTO value, RoleUserValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(true);

        var result = await sut.Validate(value);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RoleAlreadyHasUser([Frozen] Mock<IRoleUserRepository> repoMock, RoleUserDTO value, RoleUserValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleHasUser(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

        var result = await sut.Validate(value);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.ROLE_ALREADY_HAS_USER);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RoleHasNotUser([Frozen] Mock<IRoleUserRepository> repoMock, RoleUserDTO value, RoleUserValidator sut)
    {
        repoMock.Setup(x => x.CheckRoleHasUser(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

        var result = await sut.Validate(value);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.ROLE_ALREADY_HAS_USER);
    }
}
