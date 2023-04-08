using Common.DTOs;

namespace RoleSystem.Core.Interfaces.Services;

public interface IRoleFunctionService
{
    /// <summary>
    /// Add function to role with validation
    /// Publish RoleFunctionAddedEvent on created
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    Task<ResponseDTO> AddFunction(int roleId, int  functionId);

    /// <summary>
    /// Remove a function from a role
    /// Publish RoleFunctionRemovedEvent on deleted
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    Task<ResponseDTO> RemoveFunction(int roleId, int functionId);
}
