using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class AudienceConfiguration : IEntityTypeConfiguration<Audience>
{
    public void Configure(EntityTypeBuilder<Audience> builder)
    {
        builder.ToTable("Audiences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("nvarchar(255)").IsRequired();
        builder.Property(x => x.Description).HasColumnType("nvarchar(500)");
        builder.Property(x => x.Created).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.CreatedUser).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.LastEdit).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.LastEditUser).HasColumnType("nvarchar(256)").IsRequired();
    }
}
