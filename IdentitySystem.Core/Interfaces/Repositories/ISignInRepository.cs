using Microsoft.AspNetCore.Identity;

namespace IdentitySystem.Core.Interfaces.Repositories;

public interface ISignInRepository
{
    Task<IdentityUser> GetIdentityUserByEmail(string email);
    Task<IdentityUser> GetIdentityUserById(string id);
}
