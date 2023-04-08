using AutoMapper;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class AppRepository : IAppRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public AppRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<AppDTO> GetApp(Guid id)
    {
        return await context.Apps.AsNoTracking()
                                 .Where(x => x.Id == id)
                                 .Select(x => mapper.Map<AppDTO>(x))
                                 .FirstOrDefaultAsync();
    }

    public async Task CreateApp(AppDTO app)
    {
        var entity = mapper.Map<App>(app);

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateApp(AppDTO app)
    {
        await context.Apps.AsNoTracking()
                          .Where(x => x.Id == app.Id)
                          .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, app.Name)
                                                    .SetProperty(p => p.IsActive, app.IsActive)
                                                    .SetProperty(p => p.LastEditUser, app.LastEditUser)
                                                    .SetProperty(p => p.LastEdit, app.LastEdit));
    }

    public async Task DeleteApp(Guid id)
    {
        await context.Apps.AsNoTracking()
                          .Where(x => x.Id == id)
                          .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckAppExists(Guid id)
    {
        return await context.Apps.AsNoTracking().AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckAppIsActive(Guid id)
    {
        return await context.Apps.AsNoTracking().AnyAsync(x => x.Id == id && x.IsActive);
    }

    public async Task<bool> CheckClientSecrectExists(Guid id, string clientSecret)
    {
        return await context.Apps.AsNoTracking().AnyAsync(x => x.Id == id && x.IsActive && x.ClientSecret == clientSecret);
    }
}
