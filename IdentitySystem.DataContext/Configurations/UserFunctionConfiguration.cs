using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class UserFunctionConfiguration : IEntityTypeConfiguration<UserFunction>
{
    public void Configure(EntityTypeBuilder<UserFunction> builder)
    {
        builder.ToTable("UserFunctions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnType("nvarchar(450)").IsRequired();
    }
}
