using AutoFixture.Xunit2;
using Common.Interfaces;
using Moq;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Policies;
using RoleSystem.Tests.Configuration;

namespace RoleSystem.Tests.PolicyTests;

public class RolePolicyTest
{
    [Theory]
    [AutoMoqData]
    public void ApplyPolicy_NewRole([Frozen] Mock<IAuthenticatedUser> userMock, RoleDTO value, RolePolicy sut)
    {
        userMock.Setup(x => x.UserName).Returns("test");
        value.Id = 0;

        sut.ApplyPolicy(value);

        Assert.Equal(DateTime.UtcNow.Date, value.Created.Date);
        Assert.Equal(DateTime.UtcNow.Date, value.LastEdit.Date);
        Assert.Equal("test", value.CreatedUser);
        Assert.Equal("test", value.LastEditUser);
    }

    [Theory]
    [AutoMoqData]
    public void ApplyPolicy_NotNewRole([Frozen] Mock<IAuthenticatedUser> userMock, RoleDTO value, RolePolicy sut)
    {
        userMock.Setup(x => x.UserName).Returns("test");
        value.Id = 1;

        sut.ApplyPolicy(value);

        Assert.NotEqual(DateTime.UtcNow.Date, value.Created.Date);
        Assert.Equal(DateTime.UtcNow.Date, value.LastEdit.Date);
        Assert.NotEqual("test", value.CreatedUser);
        Assert.Equal("test", value.LastEditUser);
    }
}