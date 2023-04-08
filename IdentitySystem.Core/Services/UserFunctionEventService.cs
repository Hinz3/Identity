using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.Core.Interfaces.Services;

namespace IdentitySystem.Core.Services;

public class UserFunctionEventService : IUserFunctionEventService
{
    private readonly IUserFunctionRepository repository;

    public UserFunctionEventService(IUserFunctionRepository repository)
    {
        this.repository = repository;
    }

    public async Task UpdateUserFunctions(string userId, List<int> functionIds)
    {
        await repository.DeleteUserFunctions(userId);
        await repository.CreateUserFunctions(userId, functionIds);
    }
}
