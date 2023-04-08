using AutoFixture.Xunit2;
using Moq;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Providers;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Services;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.ServiceTests;

public class RoleEventServiceTest
{
    [Theory]
    [AutoMoqData]
    public async void RoleFunctionChanged_NoUsers([Frozen] Mock<IRoleUserRepository> repoMock, [Frozen] Mock<IFunctionProvider> providerMock, RoleEventService sut)
    {
        repoMock.Setup(x => x.GetRoleUserIds(It.IsAny<int>())).ReturnsAsync(new List<string>());

        await sut.RoleFunctionChanged(1);

        providerMock.Verify(x => x.GetFunctions(It.IsAny<string>()), Times.Never());
    }

    [Theory]
    [AutoMoqData]
    public async void RoleFunctionChanged_HasUsers([Frozen] Mock<IRoleUserRepository> repoMock, [Frozen] Mock<IFunctionProvider> providerMock, RoleEventService sut)
    {
        repoMock.Setup(x => x.GetRoleUserIds(It.IsAny<int>())).ReturnsAsync(new List<string>() { "test" });
        providerMock.Setup(x => x.GetFunctions(It.IsAny<string>())).ReturnsAsync(new List<FunctionDTO>());

        await sut.RoleFunctionChanged(1);

        providerMock.Verify(x => x.GetFunctions(It.IsAny<string>()), Times.Once());
    }

    [Theory]
    [AutoMoqData]
    public async void RoleFunctionRemoved_NoUsers([Frozen] Mock<IRoleUserRepository> repoMock, [Frozen] Mock<IFunctionProvider> providerMock, RoleEventService sut)
    {
        repoMock.Setup(x => x.GetRoleUserIds(It.IsAny<int>())).ReturnsAsync(new List<string>());

        await sut.RoleFunctionRemoved(1);

        repoMock.Verify(x => x.DeleteRoleUsers(It.IsAny<int>()), Times.Never());
        providerMock.Verify(x => x.GetFunctions(It.IsAny<string>()), Times.Never());
    }

    [Theory]
    [AutoMoqData]
    public async void RoleFunctionRemoved_HasUsers([Frozen] Mock<IRoleUserRepository> repoMock, [Frozen] Mock<IFunctionProvider> providerMock, RoleEventService sut)
    {
        repoMock.Setup(x => x.GetRoleUserIds(It.IsAny<int>())).ReturnsAsync(new List<string>() { "test" });
        providerMock.Setup(x => x.GetFunctions(It.IsAny<string>())).ReturnsAsync(new List<FunctionDTO>());

        await sut.RoleFunctionRemoved(1);

        repoMock.Verify(x => x.DeleteRoleUsers(It.IsAny<int>()), Times.Once());
        providerMock.Verify(x => x.GetFunctions(It.IsAny<string>()), Times.Once());
    }


    [Theory]
    [AutoMoqData]
    public async void RoleUserChanged_HasUsers([Frozen] Mock<IFunctionProvider> providerMock, RoleEventService sut)
    {
        providerMock.Setup(x => x.GetFunctions(It.IsAny<string>())).ReturnsAsync(new List<FunctionDTO>());

        await sut.RoleUserChanged("test");

        providerMock.Verify(x => x.GetFunctions(It.IsAny<string>()), Times.Once());
    }
}
