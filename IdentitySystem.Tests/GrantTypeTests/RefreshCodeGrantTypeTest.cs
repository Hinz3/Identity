using AutoFixture.Xunit2;
using IdentitySystem.Core.GrantTypes;
using IdentitySystem.Core.Interfaces.Repositories;
using Moq;
using System.Security.Authentication;

namespace IdentitySystem.Tests.GrantTypeTests;

public class RefreshCodeGrantTypeTest
{
    [Theory]
    [AutoMoqData]
    public async void GetGrantType_CouldNotGetRefreshTokenId([Frozen] Mock<IRefreshTokenRepository> authMock, RefreshCodeGrantType sut)
    {
        authMock.Setup(x => x.GetRefreshTokenId(It.IsAny<string>())).ReturnsAsync(0);

        await Assert.ThrowsAsync<InvalidCredentialException>(async () => await sut.GetGrantType(Guid.NewGuid(), "test"));
    }

    [Theory]
    [AutoMoqData]
    public async void GetGrantType_Success([Frozen] Mock<IRefreshTokenRepository> authMock, [Frozen] Mock<IRefreshTokenScopeRepository> scopeMock,
        RefreshCodeGrantType sut)
    {
        authMock.Setup(x => x.GetRefreshTokenId(It.IsAny<string>())).ReturnsAsync(1);
        authMock.Setup(x => x.GetUserId(It.IsAny<int>())).ReturnsAsync(value: "648");

        var result = await sut.GetGrantType(Guid.NewGuid(), "test");

        Assert.Equal(1, result.Id);
        authMock.Verify(x => x.DeleteRefreshToken(It.IsAny<int>()), Times.Once());
        scopeMock.Verify(x => x.DeleteRefreshTokenScopes(It.IsAny<int>()), Times.Once());
    }
}
