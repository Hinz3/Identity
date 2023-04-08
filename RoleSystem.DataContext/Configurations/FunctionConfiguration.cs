using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoleSystem.DataContext.Entities;

namespace RoleSystem.DataContext.Configurations;

public class FunctionConfiguration : IEntityTypeConfiguration<Function>
{
    public void Configure(EntityTypeBuilder<Function> builder)
    {
        builder.ToTable("Functions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("nvarchar(200)").IsRequired();
        builder.Property(x => x.Description).HasColumnType("nvarchar(500)");
        builder.Property(x => x.Created).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.CreatedUser).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.LastEdit).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.LastEditUser).HasColumnType("nvarchar(256)").IsRequired();
    }
}
