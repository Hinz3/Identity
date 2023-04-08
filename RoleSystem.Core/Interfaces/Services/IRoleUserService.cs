using Common.DTOs;

namespace RoleSystem.Core.Interfaces.Services;

public interface IRoleUserService
{
    /// <summary>
    /// Get a list of user ids in a specific role
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<ResponseDTO<List<string>>> GetRoleUsers(int roleId);

    /// <summary>
    /// Add user to a role with validation
    /// Publish RoleUserAddedEvent on created
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResponseDTO> AddUser(int roleId, string userId);

    /// <summary>
    /// Remove a user from role
    /// Publish RoleUserRemoveEvent on deleted
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResponseDTO> RemoveUser(int roleId, string userId);
}
