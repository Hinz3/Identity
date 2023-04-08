using AutoFixture.Xunit2;
using IdentitySystem.Contracts.Enums;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Models;
using IdentitySystem.Core.Validators;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentitySystem.Tests.ValidatorTests;

public class TokenParametersValidatorTest
{
    [Theory]
    [AutoMoqData]
    public async void Validate_ClientIsNull(TokenParameters parameters, TokenParametersValidator sut)
    {
        parameters.ClientId = null;

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ClientWrongFormat(TokenParameters parameters, TokenParametersValidator sut)
    {
        parameters.ClientId = "asdsa";

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ClientNotActive([Frozen] Mock<IAppRepository> repoMock, TokenParameters parameters, TokenParametersValidator sut)
    {
        repoMock.Setup(x => x.CheckAppIsActive(It.IsAny<Guid>())).ReturnsAsync(false);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_WrongGrantType(TokenParameters parameters, TokenParametersValidator sut)
    {
        parameters.ClientId = Guid.NewGuid().ToString();
        parameters.GrantType = "asdsa";

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_GRANT_TYPE);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ClientSecretNotFound([Frozen] Mock<IAppRepository> repoMock, TokenParameters parameters, TokenParametersValidator sut)
    {
        repoMock.Setup(x => x.CheckClientSecrectExists(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
        parameters.ClientId = Guid.NewGuid().ToString();

        var result = await sut.Validate(parameters);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.INVALID_CLIENT_ID);
    }
}
