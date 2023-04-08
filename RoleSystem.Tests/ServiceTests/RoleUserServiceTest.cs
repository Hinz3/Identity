using AutoFixture.Xunit2;
using Common.DTOs;
using Moq;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Services;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.ServiceTests;

public class RoleUserServiceTest
{
    [Theory]
    [AutoMoqData]
    public async void GetRoleUseres_RoleNotFound([Frozen] Mock<IRoleUserRepository> repoMock, [Frozen] Mock<IRoleRepository> roleRepo, RoleUserService sut)
    {
        roleRepo.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(false);

        var result = await sut.GetRoleUsers(1);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
        repoMock.Verify(x => x.GetRoleUserIds(It.IsAny<int>()), Times.Never());
    }

    [Theory]
    [AutoMoqData]
    public async void GetRoleUseres_Success([Frozen] Mock<IRoleUserRepository> repoMock, [Frozen] Mock<IRoleRepository> roleRepo, RoleUserService sut)
    {
        roleRepo.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(true);
        repoMock.Setup(x => x.GetRoleUserIds(It.IsAny<int>())).ReturnsAsync(new List<string> { "test" });

        var result = await sut.GetRoleUsers(1);

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        repoMock.Verify(x => x.GetRoleUserIds(It.IsAny<int>()), Times.Once());
    }

    [Theory]
    [AutoMoqData]
    public async void AddUser_ValidationErrors([Frozen] Mock<IValidator<RoleUserDTO>> validatorMock, [Frozen] Mock<IRoleUserRepository> repoMock,
        ErrorDTO error, RoleUserService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<RoleUserDTO>())).ReturnsAsync(new List<ErrorDTO> { error });

        var result = await sut.AddUser(1, "sd");

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
        repoMock.Verify(x => x.CreateRoleUser(It.IsAny<RoleUserDTO>()), Times.Never());
    }

    [Theory]
    [AutoMoqData]
    public async void AddUser_Success([Frozen] Mock<IValidator<RoleUserDTO>> validatorMock, [Frozen] Mock<IRoleUserRepository> repoMock,
        RoleUserService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<RoleUserDTO>())).ReturnsAsync(new List<ErrorDTO> { });

        var result = await sut.AddUser(1, "test");

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        repoMock.Verify(x => x.CreateRoleUser(It.IsAny<RoleUserDTO>()), Times.Once());
    }

    [Theory]
    [AutoMoqData]
    public async void RemoveUser_NotFound([Frozen] Mock<IRoleUserRepository> repoMock,
        RoleUserService sut)
    {
        repoMock.Setup(x => x.CheckRoleHasUser(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

        var result = await sut.RemoveUser(1, "asd");

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.ROLE_DO_NOT_HAVE_USER);
        repoMock.Verify(x => x.DeleteRoleUser(It.IsAny<int>(), It.IsAny<string>()), Times.Never());
    }

    [Theory]
    [AutoMoqData]
    public async void RemoveUser_Success([Frozen] Mock<IRoleUserRepository> repoMock,
        RoleUserService sut)
    {
        repoMock.Setup(x => x.CheckRoleHasUser(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

        var result = await sut.RemoveUser(1, "a");

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        repoMock.Verify(x => x.DeleteRoleUser(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
    }
}
