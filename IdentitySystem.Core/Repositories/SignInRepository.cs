using IdentitySystem.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace IdentitySystem.Core.Repositories;

public class SignInRepository : ISignInRepository
{
    private readonly SignInManager<IdentityUser> signInManager;

    public SignInRepository(SignInManager<IdentityUser> signInManager)
    {
        this.signInManager = signInManager;
    }

    public async Task<IdentityUser> GetIdentityUserByEmail(string email)
    {
        return await signInManager.UserManager.FindByEmailAsync(email);
    }

    public async Task<IdentityUser> GetIdentityUserById(string id)
    {
        return await signInManager.UserManager.FindByIdAsync(id);
    }
}
