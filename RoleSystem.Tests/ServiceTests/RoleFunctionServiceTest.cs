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

public class RoleFunctionServiceTest
{
    [Theory]
    [AutoMoqData]
    public async void AddFunction_ValidationErrors([Frozen] Mock<IValidator<RoleFunctionDTO>> validatorMock, [Frozen] Mock<IRoleFunctionRepository> repoMock, 
        ErrorDTO error, RoleFunctionService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<RoleFunctionDTO>())).ReturnsAsync(new List<ErrorDTO> { error });

        var result = await sut.AddFunction(1, 1);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
        repoMock.Verify(x => x.CreateRoleFunction(It.IsAny<RoleFunctionDTO>()), Times.Never());
    }

    [Theory]
    [AutoMoqData]
    public async void AddFunction_Success([Frozen] Mock<IValidator<RoleFunctionDTO>> validatorMock, [Frozen] Mock<IRoleFunctionRepository> repoMock,
        RoleFunctionService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<RoleFunctionDTO>())).ReturnsAsync(new List<ErrorDTO> { });

        var result = await sut.AddFunction(1, 1);

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        repoMock.Verify(x => x.CreateRoleFunction(It.IsAny<RoleFunctionDTO>()), Times.Once());
    }

    [Theory]
    [AutoMoqData]
    public async void RemoveFunction_NotFound([Frozen] Mock<IRoleFunctionRepository> repoMock,
        RoleFunctionService sut)
    {
        repoMock.Setup(x => x.CheckRoleHasFunction(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

        var result = await sut.RemoveFunction(1, 1);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.ROLE_DO_NOT_HAVE_FUNCTION);
        repoMock.Verify(x => x.DeleteRoleFunction(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
    }

    [Theory]
    [AutoMoqData]
    public async void RemoveFunction_Success([Frozen] Mock<IRoleFunctionRepository> repoMock,
        RoleFunctionService sut)
    {
        repoMock.Setup(x => x.CheckRoleHasFunction(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        var result = await sut.RemoveFunction(1, 1);

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        repoMock.Verify(x => x.DeleteRoleFunction(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
    }
}
