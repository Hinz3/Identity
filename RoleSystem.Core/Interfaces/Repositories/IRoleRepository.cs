using RoleSystem.Contracts.DTOs;

namespace RoleSystem.Core.Interfaces.Repositories;

public interface IRoleRepository
{
    /// <summary>
    /// Get a list of roles
    /// </summary>
    /// <returns></returns>
    Task<List<RoleDTO>> GetRoles();

    /// <summary>
    /// Get a specific role by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RoleDTO> GetRole(int id);

    /// <summary>
    /// Creates a role
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<RoleDTO> CreateRole(RoleDTO role);
    
    /// <summary>
    /// Update existing role if found
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    Task UpdateRole(RoleDTO role);

    /// <summary>
    /// Remove existing role if found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteRole(int id);

    /// <summary>
    /// Checks if role exists based on Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> CheckRoleExists(int id);
}
