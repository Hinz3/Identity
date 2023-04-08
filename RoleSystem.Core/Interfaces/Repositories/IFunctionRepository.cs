using RoleSystem.Contracts.DTOs;

namespace RoleSystem.Core.Interfaces.Repositories;

public interface IFunctionRepository
{
    /// <summary>
    /// Get a list of all functions
    /// </summary>
    /// <returns>List of functions</returns>
    Task<List<FunctionDTO>> GetFunctions();

    /// <summary>
    /// Get a list of functions in a role based on Role Functions table
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns>List of functions</returns>
    Task<List<FunctionDTO>> GetRoleFunctions(int roleId);

    /// <summary>
    /// Get a specific function based on id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<FunctionDTO> GetFunction(int id);

    /// <summary>
    /// Creates a function
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    Task<FunctionDTO> CreateFunction(FunctionDTO function);

    /// <summary>
    /// Update existing function if found.
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    Task UpdateFunction(FunctionDTO function);

    /// <summary>
    /// Removed existing function if found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteFunction(int id);

    /// <summary>
    /// Checks if function exists based on id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> CheckFunctionExists(int id);
}
