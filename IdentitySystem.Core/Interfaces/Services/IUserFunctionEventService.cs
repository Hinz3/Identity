namespace IdentitySystem.Core.Interfaces.Services;

public interface IUserFunctionEventService
{
    Task UpdateUserFunctions(string userId, List<int> functionIds);
}
