using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whispers.Chat.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddModerator : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
          name: "Moderators",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
            ModeratedPostIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
            DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
            DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
            CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Moderators", x => x.Id);
            table.ForeignKey(
                      name: "FK_Moderators_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

    // Create indexes for better query performance
    migrationBuilder.CreateIndex(
        name: "IX_Moderators_UserId",
        table: "Moderators",
        column: "UserId",
        unique: true); // One moderator record per user

    migrationBuilder.CreateIndex(
        name: "IX_Moderators_IsActive",
        table: "Moderators",
        column: "IsActive");

    migrationBuilder.CreateIndex(
        name: "IX_Moderators_DateCreated",
        table: "Moderators",
        column: "DateCreated");

    // Composite index for active moderators
    migrationBuilder.CreateIndex(
        name: "IX_Moderators_IsActive_DateCreated",
        table: "Moderators",
        columns: new[] { "IsActive", "DateCreated" });
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
         name: "Moderators");
  }
}
