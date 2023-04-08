using IdentitySystem.DataContext.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentitySystem.DataContext.DataContexts;

public class IdentityContext : IdentityDbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
    {
    }

    public DbSet<App> Apps { get; set; }
    public DbSet<AppUrl> AppUrls { get; set; }
    public DbSet<AuthorizationCode> AuthorizationCodes { get; set; }
    public DbSet<AuthorizationScope> AuthorizationScopes { get; set; }
    public DbSet<Scope> Scopes { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<RefreshTokenScope> RefreshTokenScopes { get; set; }
    public DbSet<UserFunction> UserFunctions { get; set; }
    public DbSet<Audience> Audiences { get; set; }
    public DbSet<ScopeAudience> ScopeAudiences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}