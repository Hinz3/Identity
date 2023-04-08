using RoleSystem.Contracts.DTOs;

namespace RoleSystem.Core.Interfaces.Repositories;

public interface IRoleUserRepository
{
    /// <summary>
    /// Get all users in a role
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns>List of user ids in role</returns>
    Task<List<string>> GetRoleUserIds(int roleId);

    /// <summary>
    /// Get a list of roles a user is in
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<int>> GetRoleIds(string userId);

    /// <summary>
    /// Adds a user to a role
    /// </summary>
    /// <param name="roleUser"></param>
    /// <returns></returns>
    Task CreateRoleUser(RoleUserDTO roleUser);

    /// <summary>
    /// Remove a user from a role
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteRoleUser(int roleId, string userId);

    /// <summary>
    /// Remove all role users from a role
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task DeleteRoleUsers(int roleId);

    /// <summary>
    /// Check if user is alreay in role
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> CheckRoleHasUser(int roleId, string userId);
}
