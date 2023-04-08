using EasyNetQ.AutoSubscribe;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.EventHandlers.RoleEvents;

public class RoleDeletedEventHandler : IConsumeAsync<RoleDeletedEvent>
{
    private readonly IRoleEventService eventService;

    public RoleDeletedEventHandler(IRoleEventService eventService)
    {
        this.eventService = eventService;
    }

    public async Task ConsumeAsync(RoleDeletedEvent message, CancellationToken cancellationToken = default)
    {
        await eventService.RoleFunctionRemoved(message.RoleId);
    }
}
