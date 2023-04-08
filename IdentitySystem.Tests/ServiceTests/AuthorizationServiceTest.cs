using AutoFixture.Xunit2;
using Common.DTOs;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Contracts.Enums;
using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Interfaces.Services;
using IdentitySystem.Core.Models;
using IdentitySystem.Core.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentitySystem.Tests.ServiceTests;

public class AuthorizationServiceTest
{
    [Theory]
    [AutoMoqData]
    public async void GenerateAuthorizationCode_ValidationError([Frozen] Mock<IValidator<LoginParameters>> validatorMock, ErrorDTO error, LoginParameters login, 
        AuthorizationService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<LoginParameters>())).ReturnsAsync(new List<ErrorDTO> { error });

        login.ClientId = Guid.NewGuid().ToString();

        var result = await sut.GenerateAuthorizationCode("test", login);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
    }

    [Theory]
    [AutoMoqData]
    public async void GenerateAuthorizationCode_UserNotFound([Frozen] Mock<IValidator<LoginParameters>> validatorMock, [Frozen] Mock<ISignInRepository> signInMock, 
        LoginParameters login,
        AuthorizationService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<LoginParameters>())).ReturnsAsync(new List<ErrorDTO> { });
        signInMock.Setup(x => x.GetIdentityUserByEmail(It.IsAny<string>())).ReturnsAsync(value: null);

        login.ClientId = Guid.NewGuid().ToString();

        var result = await sut.GenerateAuthorizationCode("test", login);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.INVALID_REQUEST);
    }

    [Theory]
    [AutoMoqData]
    public async void GenerateAuthorizationCode_ScopesNotSet([Frozen] Mock<IValidator<LoginParameters>> validatorMock, [Frozen] Mock<ISignInRepository> signInMock,
        [Frozen] Mock<IAuthorizationCodeRepository> authRepoMock, AuthorizationCodeDTO code, [Frozen] Mock<IAuthorizationScopeRepository> repoMock, IdentityUser user, 
        LoginParameters login, AuthorizationService sut)
    {
        login.Scopes = new List<string>();
        login.RedirectUrl = "https://localhost:4200";
        login.ClientId = Guid.NewGuid().ToString();

        authRepoMock.Setup(x => x.CreateAuthorizationCode(It.IsAny<AuthorizationCodeDTO>())).ReturnsAsync(code);
        validatorMock.Setup(x => x.Validate(It.IsAny<LoginParameters>())).ReturnsAsync(new List<ErrorDTO> { });
        signInMock.Setup(x => x.GetIdentityUserByEmail(It.IsAny<string>())).ReturnsAsync(value: user);

        var result = await sut.GenerateAuthorizationCode("test", login);

        repoMock.Verify(x => x.CreateAuthorizationScopes(It.IsAny<List<AuthorizationScopeDTO>>()), Times.Never);
        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void GenerateAuthorizationCode_StateNotSet([Frozen] Mock<IValidator<LoginParameters>> validatorMock, [Frozen] Mock<ISignInRepository> signInMock,
        [Frozen] Mock<IAuthorizationCodeRepository> authRepoMock, AuthorizationCodeDTO code, IdentityUser user, LoginParameters login, AuthorizationService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<LoginParameters>())).ReturnsAsync(new List<ErrorDTO> { });
        signInMock.Setup(x => x.GetIdentityUserByEmail(It.IsAny<string>())).ReturnsAsync(value: user);
        authRepoMock.Setup(x => x.CreateAuthorizationCode(It.IsAny<AuthorizationCodeDTO>())).ReturnsAsync(code);

        login.ClientId = Guid.NewGuid().ToString();
        login.RedirectUrl = "https://localhost:4200";
        login.State = null;

        var result = await sut.GenerateAuthorizationCode("test", login);

        Assert.DoesNotContain("state", result.Data);
        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void GenerateToken_ValidationError([Frozen] Mock<IValidator<TokenParameters>> validatorMock, ErrorDTO error, TokenParameters token,
        AuthorizationService sut)
    {
        validatorMock.Setup(x => x.Validate(It.IsAny<TokenParameters>())).ReturnsAsync(new List<ErrorDTO> { error });

        token.ClientId = Guid.NewGuid().ToString();

        var result = await sut.GenerateToken(token);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
    }

    [Theory]
    [AutoMoqData]
    public async void GenerateToken_CouldNotGetToken([Frozen] Mock<IValidator<TokenParameters>> validatorMock, [Frozen] Mock<ITokenGrantType> grantType, 
        [Frozen] Mock<ITokenService> tokenMock, GrantTypeResponse grantResponse, TokenParameters token, AuthorizationService sut)
    {
        grantType.Setup(x => x.GrantType).Returns(token.GrantType);
        grantType.Setup(x => x.GetGrantType(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(grantResponse);
        tokenMock.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<List<string>>())).ReturnsAsync(value: null);

        validatorMock.Setup(x => x.Validate(It.IsAny<TokenParameters>())).ReturnsAsync(new List<ErrorDTO> { });

        token.ClientId = Guid.NewGuid().ToString();

        var result = await sut.GenerateToken(token);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.INVALID_REQUEST);
    }

    [Theory]
    [AutoMoqData]
    public async void GenerateToken_Success([Frozen] Mock<IValidator<TokenParameters>> validatorMock, [Frozen] Mock<ITokenGrantType> grantType, 
        [Frozen] Mock<ITokenService> tokenMock, TokenParameters token, TokenResponseDTO response, GrantTypeResponse grantResponse, AuthorizationService sut)
    {
        grantType.Setup(x => x.GrantType).Returns(token.GrantType);
        grantType.Setup(x => x.GetGrantType(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(grantResponse);
        tokenMock.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<List<string>>())).ReturnsAsync(value: response);

        validatorMock.Setup(x => x.Validate(It.IsAny<TokenParameters>())).ReturnsAsync(new List<ErrorDTO> { });

        token.ClientId = Guid.NewGuid().ToString();

        var result = await sut.GenerateToken(token);

        Assert.True(result.Success);
    }
}
