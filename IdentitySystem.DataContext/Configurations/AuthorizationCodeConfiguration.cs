using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class AuthorizationCodeConfiguration : IEntityTypeConfiguration<AuthorizationCode>
{
    public void Configure(EntityTypeBuilder<AuthorizationCode> builder)
    {
        builder.ToTable("AuthorizationCodes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AppId).HasColumnType("uniqueidentifier").IsRequired();
        builder.Property(x => x.Code).HasColumnType("nvarchar(255)").IsRequired();
        builder.Property(x => x.UserId).HasColumnType("nvarchar(450)").IsRequired();
        builder.Property(x => x.Expire).HasColumnType("datetime").IsRequired();
    }
}
