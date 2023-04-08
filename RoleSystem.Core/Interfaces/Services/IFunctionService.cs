using Common.DTOs;
using RoleSystem.Contracts.DTOs;

namespace RoleSystem.Core.Interfaces.Services;

public interface IFunctionService
{
    /// <summary>
    /// Get a list of functions
    /// </summary>
    /// <returns></returns>
    Task<ResponseDTO<List<FunctionDTO>>> GetFunctions();

    /// <summary>
    /// Get function by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDTO<FunctionDTO>> GetFunction(int id);

    /// <summary>
    /// Create a function
    /// Publish FunctionCreatedEvent on created
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    Task<ResponseDTO<FunctionDTO>> CreateFunction(FunctionDTO function);

    /// <summary>
    /// Update a function
    /// Publish FunctionUpdatedEvent on updated
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    Task<ResponseDTO<FunctionDTO>> UpdateFunction(FunctionDTO function);

    /// <summary>
    /// Delete a function
    /// Publish FunctionDeletedEvent on deleted
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDTO> DeleteFunction(int id);
}
