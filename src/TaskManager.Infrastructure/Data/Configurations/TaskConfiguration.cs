using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
    {
        builder.ToTable("tasks", "public");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnName("description");

        builder.Property(t => t.Status)
            .IsRequired()
            .HasColumnName("status")
            .HasConversion<int>();

        builder.Property(t => t.Priority)
            .IsRequired()
            .HasColumnName("priority")
            .HasConversion<int>();

        builder.Property(t => t.DueDate)
            .IsRequired(false)
            .HasColumnName("duedate");

        builder.Property(t => t.UserId)
            .IsRequired()
            .HasColumnName("userid")
            .HasColumnType("uuid");

        builder.Property(t => t.AssignedToUserId)
            .IsRequired(false)
            .HasColumnName("assignedtouserid")
            .HasColumnType("uuid");

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnName("createdat");

        builder.Property(t => t.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updatedat");

        builder.Property(t => t.IsDeleted)
            .IsRequired()
            .HasColumnName("isdeleted")
            .HasDefaultValue(false);

        builder.HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.AssignedToUser)
            .WithMany()
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.DueDate);
        builder.HasIndex(t => t.CreatedAt);
    }
}
