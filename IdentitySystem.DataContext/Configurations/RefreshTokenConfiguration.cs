﻿using IdentitySystem.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.DataContext.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Token).HasColumnType("nvarchar(255)").IsRequired();
        builder.Property(x => x.UserId).HasColumnType("nvarchar(450)").IsRequired();
        builder.Property(x => x.Expire).HasColumnType("datetime").IsRequired();
    }
}
