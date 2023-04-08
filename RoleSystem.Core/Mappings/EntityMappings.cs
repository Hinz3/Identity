using AutoMapper;
using RoleSystem.Contracts.DTOs;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.Core.Mappings;

public class EntityMappings : Profile
{
    public EntityMappings()
    {
        CreateMap<Function, FunctionDTO>()
            .ReverseMap();

        CreateMap<Role, RoleDTO>()
            .ReverseMap();

        CreateMap<RoleFunction, RoleFunctionDTO>()
            .ReverseMap();

        CreateMap<RoleUser, RoleUserDTO>()
            .ReverseMap();
    }
}
