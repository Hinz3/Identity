using AutoFixture.Xunit2;
using IdentitySystem.Core.GrantTypes;
using IdentitySystem.Core.Interfaces.Repositories;
using Moq;
using System.Security.Authentication;

namespace IdentitySystem.Tests.GrantTypeTests;

public class AuthorizationCodeGrantTypeTest
{
    [Theory]
    [AutoMoqData]
    public async void GetGrantType_CouldNotGetAuthorizationId([Frozen] Mock<IAuthorizationCodeRepository> authMock, AuthorizationCodeGrantType sut)
    {
        authMock.Setup(x => x.GetAuthorizationId(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(0);

        await Assert.ThrowsAsync<InvalidCredentialException>(async () => await sut.GetGrantType(Guid.NewGuid(), "test"));
    }

    [Theory]
    [AutoMoqData]
    public async void GetGrantType_CouldNotGetUserId([Frozen] Mock<IAuthorizationCodeRepository> authMock, AuthorizationCodeGrantType sut)
    {
        authMock.Setup(x => x.GetAuthorizationId(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(1);
        authMock.Setup(x => x.GetUserId(It.IsAny<int>())).ReturnsAsync(value: null);

        await Assert.ThrowsAsync<InvalidCredentialException>(async () => await sut.GetGrantType(Guid.NewGuid(), "test"));
    }

    [Theory]
    [AutoMoqData]
    public async void GetGrantType_Success([Frozen] Mock<IAuthorizationCodeRepository> authMock, [Frozen] Mock<IAuthorizationScopeRepository> scopeMock, 
        AuthorizationCodeGrantType sut)
    {
        authMock.Setup(x => x.GetAuthorizationId(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(1);
        authMock.Setup(x => x.GetUserId(It.IsAny<int>())).ReturnsAsync(value: "648");

        var result = await sut.GetGrantType(Guid.NewGuid(), "test");

        Assert.Equal(1, result.Id);
        authMock.Verify(x => x.DeleteAuthorizationCode(It.IsAny<int>()), Times.Once());
        scopeMock.Verify(x => x.DeleteAuthorizationScopes(It.IsAny<int>()), Times.Once());
    }
}
