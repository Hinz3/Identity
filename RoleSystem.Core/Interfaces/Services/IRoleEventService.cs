namespace RoleSystem.Core.Interfaces.Services;

public interface IRoleEventService
{
    /// <summary>
    /// Find out what all users in role has access to and publish event
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task RoleFunctionChanged(int roleId);

    /// <summary>
    /// Role has been deleted and needs to update what users has access to
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task RoleFunctionRemoved(int roleId);

    /// <summary>
    /// Gets a list of functions user has access to and publish event
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task RoleUserChanged(string userId);
}
