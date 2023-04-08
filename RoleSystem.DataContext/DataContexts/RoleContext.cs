using Microsoft.EntityFrameworkCore;
using RoleSystem.DataContext.Entities;
using System.Reflection;

namespace RoleSystem.DataContext.DataContexts;

public class RoleContext : DbContext
{
    public RoleContext(DbContextOptions<RoleContext> options)
            : base(options)
    {
    }

    public DbSet<Function> Functions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleFunction> RoleFunctions { get; set; }
    public DbSet<RoleUser> RoleUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}