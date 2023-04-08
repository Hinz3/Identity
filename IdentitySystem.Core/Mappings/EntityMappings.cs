using AutoMapper;
using IdentitySystem.Contracts.DTOs;
using IdentitySystem.DataContext.Entities;

namespace IdentitySystem.Core.Mappings;

public class EntityMappings : Profile
{
	public EntityMappings()
	{
		CreateMap<App, AppDTO>()
			.ReverseMap();

		CreateMap<AppUrl, AppUrlDTO>()
			.ReverseMap();

		CreateMap<AuthorizationCode, AuthorizationCodeDTO>()
			.ReverseMap();

		CreateMap<AuthorizationScope, AuthorizationScopeDTO>()
			.ReverseMap();

		CreateMap<Scope, ScopeDTO>()
			.ReverseMap();

		CreateMap<RefreshToken, RefreshTokenDTO>()
			.ReverseMap();

		CreateMap<RefreshTokenScope, RefreshTokenScopeDTO>()
			.ReverseMap();

		CreateMap<Audience, AudienceDTO>()
			.ReverseMap();

		CreateMap<ScopeAudience, ScopeAudienceDTO>()
			.ReverseMap();
	}
}
