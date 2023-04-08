using AutoMapper;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.Core.Interfaces.Repositories;
using IdentitySystem.DataContext.DataContexts;
using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Core.Repositories;

public class AudienceRepository : IAudienceRepository
{
    private readonly IdentityContext context;
    private readonly IMapper mapper;

    public AudienceRepository(IdentityContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<AudienceDTO>> GetAudiences()
    {
        var query = context.Audiences.AsNoTracking();
        return await query.AsNoTracking()
                          .Select(x => mapper.Map<AudienceDTO>(x))
                          .ToListAsync();
    }

    public async Task<AudienceDTO> GetAudience(int id)
    {
        return await context.Audiences.AsNoTracking()
                                   .Where(x => x.Id == id)
                                   .Select(x => mapper.Map<AudienceDTO>(x))
                                   .FirstOrDefaultAsync();
    }

    public async Task<AudienceDTO> CreateAudience(AudienceDTO audience)
    {
        var entity = mapper.Map<Audience>(audience);
        entity.Id = 0;

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return mapper.Map<AudienceDTO>(entity);
    }

    public async Task UpdateAudience(AudienceDTO Audience)
    {
        await context.Audiences.AsNoTracking()
                            .Where(x => x.Id == Audience.Id)
                            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, Audience.Name)
                                                      .SetProperty(p => p.Description, Audience.Description)
                                                      .SetProperty(p => p.LastEdit, Audience.LastEdit)
                                                      .SetProperty(p => p.LastEditUser, Audience.LastEditUser));
    }

    public async Task DeleteAudience(int id)
    {
        await context.Audiences.AsNoTracking()
                            .Where(x => x.Id == id)
                            .ExecuteDeleteAsync();
    }

    public async Task<bool> CheckAudienceExists(int id)
    {
        return await context.Audiences.AsNoTracking().AnyAsync(x => x.Id == id);
    }

    public async Task<bool> CheckAudienceExists(string name)
    {
        return await context.Audiences.AsNoTracking().AnyAsync(x => x.Name == name);
    }
}
