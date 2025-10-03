using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "public");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("email")
            .HasConversion(
                v => v.Value,
                v => Email.From(v));

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasColumnName("passwordhash")
            .HasConversion(
                v => v.Value,
                v => PasswordHash.From(v));


        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasColumnName("isactive")
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnName("createdat");

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updatedat");

        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasColumnName("isdeleted")
            .HasDefaultValue(false);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.IsActive);
        builder.HasIndex(u => u.CreatedAt);
    }
}
