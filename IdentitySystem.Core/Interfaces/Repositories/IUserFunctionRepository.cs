namespace IdentitySystem.Core.Interfaces.Repositories;

public interface IUserFunctionRepository
{
    Task<List<int>> GetUserFunctions(string userId);
    Task CreateUserFunctions(string userId, List<int> functions);
    Task DeleteUserFunctions(string userId);
}
