using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class ScopeAudienceConfiguration : IEntityTypeConfiguration<ScopeAudience>
{
    public void Configure(EntityTypeBuilder<ScopeAudience> builder)
    {
        builder.ToTable("ScopeAudiences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Created).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.CreatedUser).HasColumnType("nvarchar(256)").IsRequired();

    }
}
