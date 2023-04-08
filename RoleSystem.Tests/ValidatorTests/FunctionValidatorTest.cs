using AutoFixture.Xunit2;
using Moq;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Validators;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.ValidatorTests;

public class FunctionValidatorTest
{
    [Theory]
    [AutoMoqData]
    public async void Validate_NameNotSet(FunctionDTO function, FunctionValidator sut)
    {
        function.Name = null;

        var result = await sut.Validate(function);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.FUNCTION_NAME_REQUIRED);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_NameIsSet(FunctionDTO function, FunctionValidator sut)
    {
        function.Name = "sad";

        var result = await sut.Validate(function);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.FUNCTION_NAME_REQUIRED);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ParentIsSame(FunctionDTO function, FunctionValidator sut)
    {
        function.ParentFunctionId = function.Id;

        var result = await sut.Validate(function);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.FUNCTION_PARENT_CANNOT_BE_SAME);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ParentIsNotSame(FunctionDTO function, FunctionValidator sut)
    {
        function.ParentFunctionId = function.Id * 2;

        var result = await sut.Validate(function);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.FUNCTION_PARENT_CANNOT_BE_SAME);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ParentNotFound([Frozen] Mock<IFunctionRepository> repoMock, FunctionDTO function, FunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(false);
        function.ParentFunctionId = 1;

        var result = await sut.Validate(function);

        Assert.Contains(result, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void Validate_ParentExists([Frozen] Mock<IFunctionRepository> repoMock, FunctionDTO function, FunctionValidator sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(true);
        function.ParentFunctionId = 1;

        var result = await sut.Validate(function);

        Assert.DoesNotContain(result, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }
}
