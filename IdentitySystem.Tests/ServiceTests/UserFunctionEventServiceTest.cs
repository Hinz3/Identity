using AutoFixture.Xunit2;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Services;
using Moq;

namespace IdentitySystem.Tests.ServiceTests;

public class UserFunctionEventServiceTest
{
    [Theory]
    [AutoMoqData]
    public async void UpdateUserFunctions_Success([Frozen] Mock<IUserFunctionRepository> repoMock, UserFunctionEventService sut)
    {
        await sut.UpdateUserFunctions("test", new List<int>());

        repoMock.Verify(x => x.DeleteUserFunctions(It.IsAny<string>()), Times.Once());
        repoMock.Verify(x => x.CreateUserFunctions(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Once());
    }
}
