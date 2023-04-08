using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.DataContext.Configurations;

public class RoleUserConfiguration : IEntityTypeConfiguration<RoleUser>
{
    public void Configure(EntityTypeBuilder<RoleUser> builder)
    {
        builder.ToTable("RoleUsers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnType("nvarchar(450)").IsRequired();
        builder.Property(x => x.Created).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.CreatedUser).HasColumnType("nvarchar(256)").IsRequired();
    }
}
