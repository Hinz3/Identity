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

public class FunctionServiceTest
{
    [Theory]
    [AutoMoqData]
    public async void GetFunctions_Success([Frozen] Mock<IFunctionRepository> repoMock, List<FunctionDTO> dtos, FunctionService sut)
    {
        repoMock.Setup(x => x.GetFunctions()).ReturnsAsync(dtos);

        var result = await sut.GetFunctions();

        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void GetFunction_NotFound([Frozen] Mock<IFunctionRepository> repoMock, FunctionService sut)
    {
        repoMock.Setup(x => x.GetFunction(It.IsAny<int>())).ReturnsAsync(value: null);

        var result = await sut.GetFunction(1);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void GetFunction_Success([Frozen] Mock<IFunctionRepository> repoMock, FunctionDTO dto, FunctionService sut)
    {
        repoMock.Setup(x => x.GetFunction(It.IsAny<int>())).ReturnsAsync(value: dto);

        var result = await sut.GetFunction(1);

        Assert.True(result.Success);
        Assert.DoesNotContain(result.Errors, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void CreateFunction_ValidationError([Frozen] Mock<IFunctionRepository> repoMock, [Frozen] Mock<IValidator<FunctionDTO>> validator, ErrorDTO error, 
        FunctionDTO dto, FunctionService sut)
    {
        validator.Setup(x => x.Validate(It.IsAny<FunctionDTO>())).ReturnsAsync(value: new List<ErrorDTO> { error });

        var result = await sut.CreateFunction(dto);

        repoMock.Verify(x => x.CreateFunction(It.IsAny<FunctionDTO>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
    }

    [Theory]
    [AutoMoqData]
    public async void CreateFunction_Success([Frozen] Mock<IFunctionRepository> repoMock, [Frozen] Mock<IValidator<FunctionDTO>> validator,
        FunctionDTO dto, FunctionService sut)
    {
        validator.Setup(x => x.Validate(It.IsAny<FunctionDTO>())).ReturnsAsync(value: new List<ErrorDTO> { });

        var result = await sut.CreateFunction(dto);

        repoMock.Verify(x => x.CreateFunction(It.IsAny<FunctionDTO>()), Times.Once());
        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void UpdateFunction_NotFound([Frozen] Mock<IFunctionRepository> repoMock, FunctionDTO dto, FunctionService sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(value: false);

        var result = await sut.UpdateFunction(dto);

        repoMock.Verify(x => x.UpdateFunction(It.IsAny<FunctionDTO>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void UpdateFunction_ValidationError([Frozen] Mock<IFunctionRepository> repoMock, [Frozen] Mock<IValidator<FunctionDTO>> validator, ErrorDTO error,
        FunctionDTO dto, FunctionService sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(value: true);
        validator.Setup(x => x.Validate(It.IsAny<FunctionDTO>())).ReturnsAsync(value: new List<ErrorDTO> { error });

        var result = await sut.UpdateFunction(dto);

        repoMock.Verify(x => x.UpdateFunction(It.IsAny<FunctionDTO>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == error.Code);
    }

    [Theory]
    [AutoMoqData]
    public async void UpdateFunction_Success([Frozen] Mock<IFunctionRepository> repoMock, [Frozen] Mock<IValidator<FunctionDTO>> validator, FunctionDTO dto, FunctionService sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(value: true);
        validator.Setup(x => x.Validate(It.IsAny<FunctionDTO>())).ReturnsAsync(value: new List<ErrorDTO> { });

        var result = await sut.UpdateFunction(dto);

        repoMock.Verify(x => x.UpdateFunction(It.IsAny<FunctionDTO>()), Times.Once());
        Assert.True(result.Success);
    }

    [Theory]
    [AutoMoqData]
    public async void DeleteFunction_NotFound([Frozen] Mock<IFunctionRepository> repoMock, FunctionService sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(value: false);

        var result = await sut.DeleteFunction(1);

        repoMock.Verify(x => x.DeleteFunction(It.IsAny<int>()), Times.Never());
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Code == (int)ErrorCode.FUNCTION_NOT_FOUND);
    }

    [Theory]
    [AutoMoqData]
    public async void DeleteFunction_Success([Frozen] Mock<IFunctionRepository> repoMock, FunctionService sut)
    {
        repoMock.Setup(x => x.CheckFunctionExists(It.IsAny<int>())).ReturnsAsync(value: true);

        var result = await sut.DeleteFunction(1);

        repoMock.Verify(x => x.DeleteFunction(It.IsAny<int>()), Times.Once());
        Assert.True(result.Success);
    }
}
