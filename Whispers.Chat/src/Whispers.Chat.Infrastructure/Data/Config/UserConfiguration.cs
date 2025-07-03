using System.Text.Json;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;

namespace Whispers.Chat.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    // Table name
    builder.ToTable("Users");

    // Primary key
    builder.HasKey(u => u.Id);

    // Properties configuration
    builder.Property(u => u.Username)
        .IsRequired()
        .HasMaxLength(256);

    builder.Property(u => u.Email)
        .IsRequired()
        .HasMaxLength(320); // Standard max email length

    builder.Property(u => u.PasswordHash)
        .IsRequired();

    // Configure LikedPostIds as JSON
    builder.Property(u => u.LikedPostIds)
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>())
        .HasColumnType("nvarchar(max)")
        .IsRequired();

    builder.Property(u => u.IsAnonymous)
        .IsRequired()
        .HasDefaultValue(false);

    // Audit properties
    builder.Property(u => u.DateCreated)
        .IsRequired()
        .HasDefaultValueSql("GETUTCDATE()");

    builder.Property(u => u.DateUpdated)
        .IsRequired(false);

    builder.Property(u => u.CreatedBy)
        .IsRequired();

    builder.Property(u => u.UpdatedBy)
        .IsRequired(false);

    // Indexes
    builder.HasIndex(u => u.Username)
        .IsUnique()
        .HasDatabaseName("IX_Users_Username");

    builder.HasIndex(u => u.Email)
        .IsUnique()
        .HasDatabaseName("IX_Users_Email");

    builder.HasIndex(u => u.IsAnonymous)
        .HasDatabaseName("IX_Users_IsAnonymous");

    builder.HasIndex(u => u.DateCreated)
        .HasDatabaseName("IX_Users_DateCreated");

    // Configure private setters (if needed)
    builder.Property(u => u.Username).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(u => u.Email).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(u => u.PasswordHash).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(u => u.LikedPostIds).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(u => u.IsAnonymous).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(u => u.DateUpdated).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(u => u.UpdatedBy).UsePropertyAccessMode(PropertyAccessMode.Field);
  }
}
