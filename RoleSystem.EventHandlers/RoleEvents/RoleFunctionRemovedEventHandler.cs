using EasyNetQ.AutoSubscribe;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.EventHandlers.RoleEvents;

public class RoleFunctionRemovedEventHandler : IConsumeAsync<RoleFunctionRemovedEvent>
{
    private readonly IRoleEventService eventService;

    public RoleFunctionRemovedEventHandler(IRoleEventService eventService)
    {
        this.eventService = eventService;
    }

    public async Task ConsumeAsync(RoleFunctionRemovedEvent message, CancellationToken cancellationToken = default)
    {
        await eventService.RoleFunctionChanged(message.RoleId);
    }
}
