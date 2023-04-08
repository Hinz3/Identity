using Common.DTOs;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Core.Interfaces;
using RoleSystem.Contracts.Enums;
using AutoMapper;
using EasyNetQ;
using RoleSystem.Events;

namespace RoleSystem.Core.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository repository;
    private readonly IValidator<RoleDTO> validator;
    private readonly IPolicy<RoleDTO> policy;
    private readonly IMapper mapper;
    private readonly IBus bus;

    public RoleService(IRoleRepository repository, IValidator<RoleDTO> validator, IPolicy<RoleDTO> policy, IMapper mapper, IBus bus)
    {
        this.repository = repository;
        this.validator = validator;
        this.policy = policy;
        this.mapper = mapper;
        this.bus = bus;
    }

    public async Task<ResponseDTO<List<RoleDTO>>> GetRoles()
    {
        var Roles = await repository.GetRoles();
        return new ResponseDTO<List<RoleDTO>>(Roles);
    }

    public async Task<ResponseDTO<RoleDTO>> GetRole(int id)
    {
        var Role = await repository.GetRole(id);
        if (Role == null)
        {
            return new ResponseDTO<RoleDTO>(new ErrorDTO((int)ErrorCode.ROLE_NOT_FOUND, "Role not found"));
        }

        return new ResponseDTO<RoleDTO>(Role);
    }

    public async Task<ResponseDTO<RoleDTO>> CreateRole(RoleDTO Role)
    {
        Role.Id = 0;

        policy.ApplyPolicy(Role);
        var errors = await validator.Validate(Role);
        if (errors.Any())
        {
            return new ResponseDTO<RoleDTO>(errors);
        }

        var created = await repository.CreateRole(Role);
        await bus.PubSub.PublishAsync(mapper.Map<RoleCreatedEvent>(created));

        return new ResponseDTO<RoleDTO>(created);
    }

    public async Task<ResponseDTO<RoleDTO>> UpdateRole(RoleDTO role)
    {
        var exists = await repository.CheckRoleExists(role.Id);
        if (!exists)
        {
            return new ResponseDTO<RoleDTO>(new ErrorDTO((int)ErrorCode.ROLE_NOT_FOUND, "Role not found"));
        }

        policy.ApplyPolicy(role);
        var errors = await validator.Validate(role);
        if (errors.Any())
        {
            return new ResponseDTO<RoleDTO>(errors);
        }

        await repository.UpdateRole(role);
        await bus.PubSub.PublishAsync(mapper.Map<RoleUpdatedEvent>(role));

        return new ResponseDTO<RoleDTO>(role);
    }

    public async Task<ResponseDTO> DeleteRole(int id)
    {
        var exists = await repository.CheckRoleExists(id);
        if (!exists)
        {
            return new ResponseDTO(new ErrorDTO((int)ErrorCode.ROLE_NOT_FOUND, "Role not found"));
        }

        await repository.DeleteRole(id);
        await bus.PubSub.PublishAsync(new RoleDeletedEvent
        {
            RoleId = id
        });

        return new ResponseDTO(true);
    }
}
