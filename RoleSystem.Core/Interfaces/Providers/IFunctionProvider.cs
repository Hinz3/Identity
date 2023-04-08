using RoleSystem.Contracts.DTOs;

namespace RoleSystem.Core.Interfaces.Providers;

public interface IFunctionProvider
{
    /// <summary>
    /// Get a list of functions a user has access to based on role
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<FunctionDTO>> GetFunctions(string userId);
}
