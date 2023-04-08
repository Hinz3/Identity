using EasyNetQ.AutoSubscribe;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.EventHandlers.RoleEvents;

public class RoleFunctionAddedEventHandler : IConsumeAsync<RoleFunctionAddedEvent>
{
    private readonly IRoleEventService eventService;

    public RoleFunctionAddedEventHandler(IRoleEventService eventService)
    {
        this.eventService = eventService;
    }

    public async Task ConsumeAsync(RoleFunctionAddedEvent message, CancellationToken cancellationToken = default)
    {
        await eventService.RoleFunctionChanged(message.RoleId);
    }
}
