using EasyNetQ.AutoSubscribe;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.EventHandlers.RoleEvents;

public class RoleUserAddedEventHandler : IConsumeAsync<RoleUserAddedEvent>
{
    private readonly IRoleEventService eventService;

    public RoleUserAddedEventHandler(IRoleEventService eventService)
    {
        this.eventService = eventService;
    }

    public async Task ConsumeAsync(RoleUserAddedEvent message, CancellationToken cancellationToken = default)
    {
        await eventService.RoleUserChanged(message.UserId);
    }
}
