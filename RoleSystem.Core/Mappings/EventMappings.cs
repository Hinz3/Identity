using AutoMapper;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Events;

namespace RoleSystem.Core.Mappings;

public class EventMappings : Profile
{
    public EventMappings()
    {
        CreateMap<FunctionDTO, FunctionCreatedEvent>()
            .ForMember(x => x.FunctionId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.ParentFunctionId, opt => opt.MapFrom(x => x.ParentFunctionId))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
            .ReverseMap();

        CreateMap<FunctionDTO, FunctionUpdatedEvent>()
            .ForMember(x => x.FunctionId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.ParentFunctionId, opt => opt.MapFrom(x => x.ParentFunctionId))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
            .ReverseMap();

        CreateMap<RoleDTO, RoleCreatedEvent>()
            .ForMember(x => x.RoleId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
            .ReverseMap();

        CreateMap<RoleDTO, RoleUpdatedEvent>()
            .ForMember(x => x.RoleId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
            .ReverseMap();

        CreateMap<RoleUserDTO, RoleUserAddedEvent>()
            .ForMember(x => x.RoleId, opt => opt.MapFrom(x => x.RoleId))
            .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.UserId))
            .ReverseMap();

        CreateMap<RoleFunctionDTO, RoleFunctionAddedEvent>()
            .ForMember(x => x.RoleId, opt => opt.MapFrom(x => x.RoleId))
            .ForMember(x => x.FunctionId, opt => opt.MapFrom(x => x.FunctionId))
            .ReverseMap();
    }
}
