using Common.Interfaces;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces;

namespace RoleSystem.Core.Policies;

public class FunctionPolicy : IPolicy<FunctionDTO>
{
    private readonly IAuthenticatedUser authenticatedUser;

    public FunctionPolicy(IAuthenticatedUser authenticatedUser)
    {
        this.authenticatedUser = authenticatedUser;
    }

    public void ApplyPolicy(FunctionDTO value)
    {
        if (value.Id == 0)
        {
            value.Created = DateTime.UtcNow;
            value.CreatedUser = authenticatedUser.UserName;
        }

        value.LastEdit = DateTime.UtcNow;
        value.LastEditUser = authenticatedUser.UserName;
    }
}
