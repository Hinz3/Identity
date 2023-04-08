using AutoFixture.Xunit2;
using IdentitySystem.Contracts.Enums;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Models;
using IdentitySystem.Core.Validators;
using Moq;

namespace IdentitySystem.Tests.ValidatorTests;

public class LoginParameterValidatorTest
{
    [Theory]
    [AutoMoqData]
    public async void Validate_ClientIsNull(LoginParameters parameters, LoginParameterValidator sut)
    {
        parameters.ClientId = null;

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ClientWrongFormat(LoginParameters parameters, LoginParameterValidator sut)
    {
        parameters.ClientId = "asdsa";

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ClientNotActive([Frozen] Mock<IAppRepository> repoMock, LoginParameters parameters, LoginParameterValidator sut)
    {
        repoMock.Setup(x => x.CheckAppIsActive(It.IsAny<Guid>())).ReturnsAsync(false);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ClientIsActive([Frozen] Mock<IAppRepository> repoMock, LoginParameters parameters, LoginParameterValidator sut)
    {
        repoMock.Setup(x => x.CheckAppIsActive(It.IsAny<Guid>())).ReturnsAsync(true);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RedirectUrlIsNull(LoginParameters parameters, LoginParameterValidator sut)
    {
        parameters.RedirectUrl = null;
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_REDIRECT_URL);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RedirectUrlNotFound([Frozen] Mock<IAppUrlRepository> repoMock, LoginParameters parameters, LoginParameterValidator sut)
    {
        repoMock.Setup(x => x.CheckUrlExists(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_REDIRECT_URL);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_RedirectUrlFound([Frozen] Mock<IAppUrlRepository> repoMock, LoginParameters parameters, LoginParameterValidator sut)
    {
        repoMock.Setup(x => x.CheckUrlExists(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.INVALID_REDIRECT_URL);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ScopeNotActive([Frozen] Mock<IScopeRepository> repoMock, LoginParameters parameters, LoginParameterValidator sut)
    {
        repoMock.Setup(x => x.CheckScopeIsActive(It.IsAny<string>())).ReturnsAsync(false);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_SCOPE);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ScopeIsActive([Frozen] Mock<IScopeRepository> repoMock, LoginParameters parameters, LoginParameterValidator sut)
    {
        repoMock.Setup(x => x.CheckScopeIsActive(It.IsAny<string>())).ReturnsAsync(true);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.INVALID_SCOPE);
    }
}
