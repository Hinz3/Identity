using EFCore.BulkExtensions;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class UserFunctionRepository : IUserFunctionRepository
{
    private readonly IdentityContext context;

    public UserFunctionRepository(IdentityContext context)
    {
        this.context = context;
    }

    public async Task<List<int>> GetUserFunctions(string userId)
    {
        return await context.UserFunctions.AsNoTracking()
                                          .Where(x => x.UserId == userId)
                                          .Select(x => x.FunctionId)
                                          .ToListAsync();
    }

    public async Task CreateUserFunctions(string userId, List<int> functions)
    {
        var entities = new List<UserFunction>();
        functions.ForEach(function =>
        {
            entities.Add(new UserFunction
            {
                Id = 0,
                UserId = userId,
                FunctionId = function,
            });
        });

        await context.BulkInsertAsync(entities);
    }

    public async Task DeleteUserFunctions(string userId)
    {
        await context.UserFunctions.AsNoTracking()
                                   .Where(x => x.UserId == userId)
                                   .ExecuteDeleteAsync();
    }

}
