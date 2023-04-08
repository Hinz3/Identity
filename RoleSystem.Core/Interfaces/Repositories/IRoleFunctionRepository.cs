using RoleSystem.Contracts.DTOs;

namespace RoleSystem.Core.Interfaces.Repositories;

public interface IRoleFunctionRepository
{
    /// <summary>
    /// Get a grouped list of all functions grouped by role id
    /// </summary>
    /// <returns></returns>
    Task<Dictionary<int, List<int>>> GetAllRoleFunctions();

    /// <summary>
    /// Adds a function role
    /// </summary>
    /// <param name="roleFunction"></param>
    /// <returns></returns>
    Task CreateRoleFunction(RoleFunctionDTO roleFunction);

    /// <summary>
    /// Remove a function from a role if found
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    Task DeleteRoleFunction(int roleId, int functionId);

    /// <summary>
    /// Checks if a role has function
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    Task<bool> CheckRoleHasFunction(int roleId, int functionId);
}
