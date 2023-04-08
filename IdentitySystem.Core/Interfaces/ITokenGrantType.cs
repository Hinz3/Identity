using IdentitySystem.Core.Models;

namespace IdentitySystem.Core.Interfaces;

public interface ITokenGrantType
{
    string GrantType { get; }

    Task<GrantTypeResponse> GetGrantType(Guid appId, string code);
}
