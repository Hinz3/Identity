using AutoMapper;
using Common.DTOs;
using Common.Interfaces;
using EasyNetQ;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.Core.Services;

public class RoleUserService : IRoleUserService
{
    private readonly IRoleUserRepository repository;
    private readonly IValidator<RoleUserDTO> validator;
    private readonly IAuthenticatedUser authenticatedUser;
    private readonly IRoleRepository roleRepository;
    private readonly IMapper mapper;
    private readonly IBus bus;

    public RoleUserService(IRoleUserRepository repository, IValidator<RoleUserDTO> validator, IAuthenticatedUser authenticatedUser,
        IRoleRepository roleRepository, IMapper mapper, IBus bus)
    {
        this.repository = repository;
        this.validator = validator;
        this.authenticatedUser = authenticatedUser;
        this.roleRepository = roleRepository;
        this.mapper = mapper;
        this.bus = bus;
    }

    public async Task<ResponseDTO<List<string>>> GetRoleUsers(int roleId)
    {
        var roleExists = await roleRepository.CheckRoleExists(roleId);
        if (!roleExists)
        {
            return new ResponseDTO<List<string>>(new ErrorDTO((int)ErrorCode.ROLE_NOT_FOUND, "Role not found"));
        }

        var roleUsers = await repository.GetRoleUserIds(roleId);
        return new ResponseDTO<List<string>>(roleUsers);
    }

    public async Task<ResponseDTO> AddUser(int roleId, string userId)
    {
        var roleUser = new RoleUserDTO
        {
            Id = 0,
            RoleId = roleId,
            UserId = userId,
            Created = DateTime.UtcNow,
            CreatedUser = authenticatedUser.UserName
        };

        var errors = await validator.Validate(roleUser);
        if (errors.Any())
        {
            return new ResponseDTO(errors);
        }
        await repository.CreateRoleUser(roleUser);
        await bus.PubSub.PublishAsync(mapper.Map<RoleUserAddedEvent>(roleUser));

        return new ResponseDTO(true);
    }

    public async Task<ResponseDTO> RemoveUser(int roleId, string userId)
    {
        var exists = await repository.CheckRoleHasUser(roleId, userId);
        if (!exists)
        {
            return new ResponseDTO(new ErrorDTO((int)ErrorCode.ROLE_DO_NOT_HAVE_USER, "User not found"));
        }

        await repository.DeleteRoleUser(roleId, userId);
        await bus.PubSub.PublishAsync(new RoleUserAddedEvent
        {
            RoleId = roleId,
            UserId = userId
        });

        return new ResponseDTO(true);
    }
}
