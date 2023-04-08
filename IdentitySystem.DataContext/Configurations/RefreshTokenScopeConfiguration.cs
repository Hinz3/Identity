using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class RefreshTokenScopeConfiguration : IEntityTypeConfiguration<RefreshTokenScope>
{
    public void Configure(EntityTypeBuilder<RefreshTokenScope> builder)
    {
        builder.ToTable("RefreshTokenScopes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Scope).HasColumnType("nvarchar(200)").IsRequired();
    }
}
