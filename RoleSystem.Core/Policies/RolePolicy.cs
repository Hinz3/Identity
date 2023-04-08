using Common.Interfaces;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces;

namespace RoleSystem.Core.Policies;

public class RolePolicy : IPolicy<RoleDTO>
{
    private readonly IAuthenticatedUser authenticatedUser;

    public RolePolicy(IAuthenticatedUser authenticatedUser)
    {
        this.authenticatedUser = authenticatedUser;
    }

    public void ApplyPolicy(RoleDTO value)
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
