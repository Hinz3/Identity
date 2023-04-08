using EasyNetQ;
using RoleSystem.Core.Interfaces.Providers;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.Core.Services;

public class RoleEventService : IRoleEventService
{
    private readonly IRoleUserRepository roleUserRepository;
    private readonly IFunctionProvider functionProvider;
    private readonly IBus bus;

    public RoleEventService(IRoleUserRepository roleUserRepository, IFunctionProvider functionProvider, IBus bus)
    {
        this.roleUserRepository = roleUserRepository;
        this.functionProvider = functionProvider;
        this.bus = bus;
    }

    public async Task RoleFunctionChanged(int roleId)
    {
        var users = await roleUserRepository.GetRoleUserIds(roleId);
        if (!users.Any())
        {
            return;
        }

        var events = new List<UserFunctionsEvent>();
        foreach (var user in users)
        {
            var functions = await functionProvider.GetFunctions(user);
            var functionIds = functions.Select(x => x.Id).ToList();

            events.Add(new UserFunctionsEvent
            {
                UserId = user,
                AssignedFunctions = functionIds,
            });
        }

        await bus.PubSub.PublishAsync(events);
    }

    public async Task RoleFunctionRemoved(int roleId)
    {
        var roleUsers = await roleUserRepository.GetRoleUserIds(roleId);
        if (!roleUsers.Any())
        {
            return;
        }

        var events = new List<UserFunctionsEvent>();
        foreach (var user in roleUsers)
        {
            var functions = await functionProvider.GetFunctions(user);
            var functionIds = functions.Select(x => x.Id).ToList();

            events.Add(new UserFunctionsEvent
            {
                UserId = user,
                AssignedFunctions = functionIds,
            });
        }

        await roleUserRepository.DeleteRoleUsers(roleId);

        await bus.PubSub.PublishAsync(events);
    }

    public async Task RoleUserChanged(string userId)
    {
        var functions = await functionProvider.GetFunctions(userId);
        var functionIds = functions.Select(x => x.Id).ToList();

        var functionEvent = new UserFunctionsEvent
        {
            UserId = userId,
            AssignedFunctions = functionIds,
        };

        await bus.PubSub.PublishAsync(functionEvent);
    }
}
