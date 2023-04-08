using AutoFixture.Xunit2;
using Moq;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Providers;
using RoleSystem.Tests.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleSystem.Tests.ProviderTests;

public class FunctionProviderTest
{
    [Theory]
    [AutoMoqData]
    public async void GetFunctions_NoRoles([Frozen] Mock<IRoleUserRepository> roleUserRepo, FunctionProvider sut)
    {
        roleUserRepo.Setup(x => x.GetRoleIds(It.IsAny<string>())).ReturnsAsync(new List<int>());
        
        var result = await sut.GetFunctions("test");

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqData]
    public async void GetFunctions_NoFunctionsInRole([Frozen] Mock<IRoleUserRepository> roleUserRepo, [Frozen] Mock<IRoleFunctionRepository> rfRepo, 
        [Frozen] Mock<IFunctionRepository> functionRepo, FunctionProvider sut)
    {
        roleUserRepo.Setup(x => x.GetRoleIds(It.IsAny<string>())).ReturnsAsync(new List<int>() { 1 });
        rfRepo.Setup(x => x.GetAllRoleFunctions()).ReturnsAsync(new Dictionary<int, List<int>>());
        functionRepo.Setup(x => x.GetFunctions()).ReturnsAsync(new List<FunctionDTO>());

        var result = await sut.GetFunctions("test");

        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqData]
    public async void GetFunctions_ParentFunction([Frozen] Mock<IRoleUserRepository> roleUserRepo, [Frozen] Mock<IRoleFunctionRepository> rfRepo,
        [Frozen] Mock<IFunctionRepository> functionRepo, FunctionDTO function, FunctionDTO parentFunction, FunctionProvider sut)
    {
        parentFunction.ParentFunctionId = null;
        function.ParentFunctionId = parentFunction.Id;

        roleUserRepo.Setup(x => x.GetRoleIds(It.IsAny<string>())).ReturnsAsync(new List<int>() { 1 });
        rfRepo.Setup(x => x.GetAllRoleFunctions()).ReturnsAsync(new Dictionary<int, List<int>> { { 1, new List<int> { parentFunction.Id } } });
        functionRepo.Setup(x => x.GetFunctions()).ReturnsAsync(new List<FunctionDTO> { function, parentFunction });

        var result = await sut.GetFunctions("test");

        Assert.Equal(2, result.Count);
    }
}
