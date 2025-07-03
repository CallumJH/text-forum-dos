using System.Text.Json;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;
using Whispers.Chat.Core.BoundedContexts.Posts;

namespace Whispers.Chat.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
  public void Configure(EntityTypeBuilder<Post> builder)
  {
    // Table name
    builder.ToTable("Posts");

    // Primary key
    builder.HasKey(p => p.Id);

    // Properties configuration
    builder.Property(p => p.Title)
        .IsRequired()
        .HasMaxLength(500);

    builder.Property(p => p.Content)
        .IsRequired()
        .HasColumnType("nvarchar(max)");

    builder.Property(p => p.AuthorId)
        .IsRequired();

    // Configure LikedByUserIds as JSON (using backing field)
    builder.Property<List<Guid>>("_likedByUserIds")
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>())
        .HasColumnName("LikedByUserIds")
        .HasColumnType("nvarchar(max)")
        .IsRequired();

    // Configure Tags as JSON
    builder.Property(p => p.Tags)
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
        .HasColumnType("nvarchar(max)")
        .IsRequired();

    builder.Property(p => p.IsMisleading)
        .IsRequired()
        .HasDefaultValue(false);

    builder.Property(p => p.IsFalseInformation)
        .IsRequired()
        .HasDefaultValue(false);

    // Audit properties
    builder.Property(p => p.DateCreated)
        .IsRequired()
        .HasDefaultValueSql("GETUTCDATE()");

    builder.Property(p => p.DateUpdated)
        .IsRequired(false);

    builder.Property(p => p.CreatedBy)
        .IsRequired();

    builder.Property(p => p.UpdatedBy)
        .IsRequired(false);

    // Ignore computed properties (they're calculated, not stored)
    builder.Ignore(p => p.LikeCount);
    builder.Ignore(p => p.Comments);
    builder.Ignore(p => p.LikedByUserIds);

    // Relationships
    builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(p => p.AuthorId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure Comments relationship (assuming Comment has PostId)
    builder.HasMany("_comments")
        .WithOne()
        .HasForeignKey("PostId")
        .OnDelete(DeleteBehavior.Cascade);

    // Indexes
    builder.HasIndex(p => p.AuthorId)
        .HasDatabaseName("IX_Posts_AuthorId");

    builder.HasIndex(p => p.DateCreated)
        .HasDatabaseName("IX_Posts_DateCreated");

    builder.HasIndex(p => p.Title)
        .HasDatabaseName("IX_Posts_Title");

    builder.HasIndex(p => p.IsMisleading)
        .HasDatabaseName("IX_Posts_IsMisleading");

    builder.HasIndex(p => p.IsFalseInformation)
        .HasDatabaseName("IX_Posts_IsFalseInformation");

    // Composite index for filtering flagged content
    builder.HasIndex(p => new { p.IsMisleading, p.IsFalseInformation })
        .HasDatabaseName("IX_Posts_IsMisleading_IsFalseInformation");

    // Configure private setters
    builder.Property(p => p.Title).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(p => p.Content).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(p => p.AuthorId).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(p => p.IsMisleading).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(p => p.IsFalseInformation).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(p => p.Tags).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(p => p.DateUpdated).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(p => p.UpdatedBy).UsePropertyAccessMode(PropertyAccessMode.Field);
  }
}
