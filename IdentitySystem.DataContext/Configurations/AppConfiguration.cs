using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class AppConfiguration : IEntityTypeConfiguration<App>
{
    public void Configure(EntityTypeBuilder<App> builder)
    {
        builder.ToTable("Apps");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("varchar(200)").IsRequired();
        builder.Property(x => x.ClientSecret).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("bit");
        builder.Property(x => x.Created).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.CreatedUser).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.LastEdit).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.LastEditUser).HasColumnType("nvarchar(256)").IsRequired();
    }
}
