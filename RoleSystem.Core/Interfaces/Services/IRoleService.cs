using Common.DTOs;
using RoleSystem.Contracts.DTOs;

namespace RoleSystem.Core.Interfaces.Services;

public interface IRoleService
{
    /// <summary>
    /// Get a list of Roles
    /// </summary>
    /// <returns></returns>
    Task<ResponseDTO<List<RoleDTO>>> GetRoles();

    /// <summary>
    /// Get Role by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDTO<RoleDTO>> GetRole(int id);

    /// <summary>
    /// Create a Role
    /// Publish RoleCreatedEvent on created
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<ResponseDTO<RoleDTO>> CreateRole(RoleDTO role);

    /// <summary>
    /// Update a Role
    /// Publish RoleUpdatedEvent on updated
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<ResponseDTO<RoleDTO>> UpdateRole(RoleDTO role);

    /// <summary>
    /// Delete a Role
    /// Publish RoleDeletedEvent on deleted
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDTO> DeleteRole(int id);
}
