using AutoMapper;
using Common.DTOs;
using Common.Interfaces;
using EasyNetQ;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.DataContext.Entities;
using RoleSystem.Events;

namespace RoleSystem.Core.Services;

public class RoleFunctionService : IRoleFunctionService
{
    private readonly IRoleFunctionRepository repository;
    private readonly IValidator<RoleFunctionDTO> validator;
    private readonly IAuthenticatedUser authenticatedUser;
    private readonly IMapper mapper;
    private readonly IBus bus;

    public RoleFunctionService(IRoleFunctionRepository repository, IValidator<RoleFunctionDTO> validator, IAuthenticatedUser authenticatedUser,
        IMapper mapper, IBus bus)
    {
        this.repository = repository;
        this.validator = validator;
        this.authenticatedUser = authenticatedUser;
        this.mapper = mapper;
        this.bus = bus;
    }

    public async Task<ResponseDTO> AddFunction(int roleId, int functionId)
    {
        var roleFunction = new RoleFunctionDTO
        {
            Id = 0,
            RoleId = roleId,
            FunctionId = functionId,
            Created = DateTime.UtcNow,
            CreatedUser = authenticatedUser.UserName
        };

        var errors = await validator.Validate(roleFunction);
        if (errors.Any())
        {
            return new ResponseDTO(errors);
        }

        await repository.CreateRoleFunction(roleFunction);
        await bus.PubSub.PublishAsync(mapper.Map<RoleFunctionAddedEvent>(roleFunction));

        return new ResponseDTO(true);
    }

    public async Task<ResponseDTO> RemoveFunction(int roleId, int functionId)
    {
        var exists = await repository.CheckRoleHasFunction(roleId, functionId);
        if (!exists)
        {
            return new ResponseDTO(new ErrorDTO((int)ErrorCode.ROLE_DO_NOT_HAVE_FUNCTION, "Role does not have function"));
        }

        await repository.DeleteRoleFunction(roleId, functionId);
        await bus.PubSub.PublishAsync(new RoleFunctionRemovedEvent
        {
            RoleId = roleId,
            FunctionId = functionId
        });

        return new ResponseDTO(true);
    }
}
