using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class AppUrlRepository : IAppUrlRepository
{
    private readonly IdentityContext context;

    public AppUrlRepository(IdentityContext context)
    {
        this.context = context;
    }

    public async Task<bool> CheckUrlExists(Guid appId, string url)
    {
        return await context.AppUrls.AsNoTracking().AnyAsync(x => x.AppId == appId && x.Url == url);
    }
}
