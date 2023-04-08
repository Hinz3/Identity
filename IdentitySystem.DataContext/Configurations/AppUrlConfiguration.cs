using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class AppUrlConfiguration : IEntityTypeConfiguration<AppUrl>
{
    public void Configure(EntityTypeBuilder<AppUrl> builder)
    {
        builder.ToTable("AppUrls");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AppId).HasColumnType("uniqueidentifier").IsRequired();
        builder.Property(x => x.Url).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.Created).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.CreatedUser).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.LastEdit).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.LastEditUser).HasColumnType("nvarchar(256)").IsRequired();
    }
}
