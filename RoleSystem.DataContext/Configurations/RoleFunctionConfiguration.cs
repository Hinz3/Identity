using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.DataContext.Configurations;

public class RoleFunctionConfiguration : IEntityTypeConfiguration<RoleFunction>
{
    public void Configure(EntityTypeBuilder<RoleFunction> builder)
    {
        builder.ToTable("RoleFunctions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Created).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.CreatedUser).HasColumnType("nvarchar(256)").IsRequired();
    }
}
