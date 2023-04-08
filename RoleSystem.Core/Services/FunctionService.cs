using AutoMapper;
using Common.DTOs;
using EasyNetQ;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Contracts.Enums;
using RoleSystem.Core.Interfaces;
using RoleSystem.Core.Interfaces.Repositories;
using RoleSystem.Core.Interfaces.Services;
using RoleSystem.Events;

namespace RoleSystem.Core.Services;

public class FunctionService : IFunctionService
{
    private readonly IFunctionRepository repository;
    private readonly IValidator<FunctionDTO> validator;
    private readonly IPolicy<FunctionDTO> policy;
    private readonly IMapper mapper;
    private readonly IBus bus;

    public FunctionService(IFunctionRepository repository, IValidator<FunctionDTO> validator, IPolicy<FunctionDTO> policy, IMapper mapper, IBus bus)
    {
        this.repository = repository;
        this.validator = validator;
        this.policy = policy;
        this.mapper = mapper;
        this.bus = bus;
    }

    public async Task<ResponseDTO<List<FunctionDTO>>> GetFunctions()
    {
        var functions = await repository.GetFunctions();
        return new ResponseDTO<List<FunctionDTO>>(functions);
    }

    public async Task<ResponseDTO<FunctionDTO>> GetFunction(int id)
    {
        var function = await repository.GetFunction(id);
        if (function == null)
        {
            return new ResponseDTO<FunctionDTO>(new ErrorDTO((int)ErrorCode.FUNCTION_NOT_FOUND, "Function not found"));
        }

        return new ResponseDTO<FunctionDTO>(function);
    }

    public async Task<ResponseDTO<FunctionDTO>> CreateFunction(FunctionDTO function)
    {
        function.Id = 0;

        policy.ApplyPolicy(function);
        var errors = await validator.Validate(function);
        if (errors.Any())
        {
            return new ResponseDTO<FunctionDTO>(errors);
        }

        var created = await repository.CreateFunction(function);
        await bus.PubSub.PublishAsync(mapper.Map<FunctionCreatedEvent>(created));

        return new ResponseDTO<FunctionDTO>(created);
    }

    public async Task<ResponseDTO<FunctionDTO>> UpdateFunction(FunctionDTO function)
    {
        var exists = await repository.CheckFunctionExists(function.Id);
        if (!exists)
        {
            return new ResponseDTO<FunctionDTO>(new ErrorDTO((int)ErrorCode.FUNCTION_NOT_FOUND, "Function not found"));
        }

        policy.ApplyPolicy(function);
        var errors = await validator.Validate(function);
        if (errors.Any())
        {
            return new ResponseDTO<FunctionDTO>(errors);
        }

        await repository.UpdateFunction(function);
        await bus.PubSub.PublishAsync(mapper.Map<FunctionUpdatedEvent>(function));

        return new ResponseDTO<FunctionDTO>(function);
    }

    public async Task<ResponseDTO> DeleteFunction(int id)
    {
        var exists = await repository.CheckFunctionExists(id);
        if (!exists)
        {
            return new ResponseDTO(new ErrorDTO((int)ErrorCode.FUNCTION_NOT_FOUND, "Function not found"));
        }

        await repository.DeleteFunction(id);

        await bus.PubSub.PublishAsync(new FunctionDeletedEvent
        {
            FunctionId = id,
        });

        return new ResponseDTO(true);
    }
}
