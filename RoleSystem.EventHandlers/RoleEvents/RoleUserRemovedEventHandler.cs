using EasyNetQ.AutoSubscribe;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.EventHandlers.RoleEvents;

public class RoleUserRemovedEventHandler : IConsumeAsync<RoleUserRemovedEvent>
{
    private readonly IRoleEventService eventService;

    public RoleUserRemovedEventHandler(IRoleEventService eventService)
    {
        this.eventService = eventService;
    }

    public async Task ConsumeAsync(RoleUserRemovedEvent message, CancellationToken cancellationToken = default)
    {
        await eventService.RoleUserChanged(message.UserId);
    }
}
