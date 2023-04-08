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

public class RoleServiceTest
{
    [Theory]
    [AutoMoqData]
    public async void GetRoles_Success([Frozen] Mock<IRoleRepository> repoMock, List<RoleDTO> dtos, RoleService sut)
    {
        repoMock.Setup(x => x.GetRoles()).ReturnsAsync(dtos);

        var result = await sut.GetRoles();

        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void GetRole_NotFound([Frozen] Mock<IRoleRepository> repoMock, RoleService sut)
    {
        repoMock.Setup(x => x.GetRole(It.IsAny<int>())).ReturnsAsync(value: null);

        var result = await sut.GetRole(1);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void GetRole_Success([Frozen] Mock<IRoleRepository> repoMock, RoleDTO dto, RoleService sut)
    {
        repoMock.Setup(x => x.GetRole(It.IsAny<int>())).ReturnsAsync(value: dto);

        var result = await sut.GetRole(1);

        Assert.True(result.Success);
        Assert.DoesNotContain(result.Errors, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void CreateRole_ValidationError([Frozen] Mock<IRoleRepository> repoMock, [Frozen] Mock<IValidator<RoleDTO>> validator, ErrorDTO error,
        RoleDTO dto, RoleService sut)
    {
        validator.Setup(x => x.Validate(It.IsAny<RoleDTO>())).ReturnsAsync(value: new List<ErrorDTO> { error });

        var result = await sut.CreateRole(dto);

        repoMock.Verify(x => x.CreateRole(It.IsAny<RoleDTO>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
    }

    [Theory]
    [AutoMoqData]
    public async void CreateRole_Success([Frozen] Mock<IRoleRepository> repoMock, [Frozen] Mock<IValidator<RoleDTO>> validator,
        RoleDTO dto, RoleService sut)
    {
        validator.Setup(x => x.Validate(It.IsAny<RoleDTO>())).ReturnsAsync(value: new List<ErrorDTO> { });

        var result = await sut.CreateRole(dto);

        repoMock.Verify(x => x.CreateRole(It.IsAny<RoleDTO>()), Times.Once());
        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void UpdateRole_NotFound([Frozen] Mock<IRoleRepository> repoMock, RoleDTO dto, RoleService sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(value: false);

        var result = await sut.UpdateRole(dto);

        repoMock.Verify(x => x.UpdateRole(It.IsAny<RoleDTO>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void UpdateRole_ValidationError([Frozen] Mock<IRoleRepository> repoMock, [Frozen] Mock<IValidator<RoleDTO>> validator, ErrorDTO error,
        RoleDTO dto, RoleService sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(value: true);
        validator.Setup(x => x.Validate(It.IsAny<RoleDTO>())).ReturnsAsync(value: new List<ErrorDTO> { error });

        var result = await sut.UpdateRole(dto);

        repoMock.Verify(x => x.UpdateRole(It.IsAny<RoleDTO>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
    }

    [Theory]
    [AutoMoqData]
    public async void UpdateRole_Success([Frozen] Mock<IRoleRepository> repoMock, [Frozen] Mock<IValidator<RoleDTO>> validator, RoleDTO dto, RoleService sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(value: true);
        validator.Setup(x => x.Validate(It.IsAny<RoleDTO>())).ReturnsAsync(value: new List<ErrorDTO> { });

        var result = await sut.UpdateRole(dto);

        repoMock.Verify(x => x.UpdateRole(It.IsAny<RoleDTO>()), Times.Once());
        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void DeleteRole_NotFound([Frozen] Mock<IRoleRepository> repoMock, RoleService sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(value: false);

        var result = await sut.DeleteRole(1);

        repoMock.Verify(x => x.DeleteRole(It.IsAny<int>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.ROLE_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void DeleteRole_Success([Frozen] Mock<IRoleRepository> repoMock, RoleService sut)
    {
        repoMock.Setup(x => x.CheckRoleExists(It.IsAny<int>())).ReturnsAsync(value: true);

        var result = await sut.DeleteRole(1);

        repoMock.Verify(x => x.DeleteRole(It.IsAny<int>()), Times.Once());
        Assert.True(result.Success);
    }
}
