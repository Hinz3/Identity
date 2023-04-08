using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class AuthorizationScopeConfiguration : IEntityTypeConfiguration<AuthorizationScope>
{
    public void Configure(EntityTypeBuilder<AuthorizationScope> builder)
    {
        builder.ToTable("AuthorizationScopes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Scope).HasColumnType("nvarchar(200)").IsRequired();
    }
}
