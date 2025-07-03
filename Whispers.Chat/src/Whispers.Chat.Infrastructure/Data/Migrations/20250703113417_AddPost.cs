using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whispers.Chat.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddPost : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                  Content = table.Column<string>(type: "nvarchar(256)", nullable: false),
                  AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  LikedByUserIds = table.Column<string>(type: "nvarchar(256)", nullable: false),
                  IsMisleading = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                  IsFalseInformation = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                  Tags = table.Column<string>(type: "nvarchar(256)", nullable: false),
                  DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                  DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                  CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                  table.PrimaryKey("PK_Posts", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Posts_Users_AuthorId",
                      column: x => x.AuthorId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
                });

    // Create indexes for better query performance
    migrationBuilder.CreateIndex(
        name: "IX_Posts_AuthorId",
        table: "Posts",
        column: "AuthorId");

    migrationBuilder.CreateIndex(
        name: "IX_Posts_DateCreated",
        table: "Posts",
        column: "DateCreated");

    migrationBuilder.CreateIndex(
        name: "IX_Posts_Title",
        table: "Posts",
        column: "Title");

    migrationBuilder.CreateIndex(
        name: "IX_Posts_IsMisleading",
        table: "Posts",
        column: "IsMisleading");

    migrationBuilder.CreateIndex(
        name: "IX_Posts_IsFalseInformation",
        table: "Posts",
        column: "IsFalseInformation");

    // Composite index for filtering flagged content
    migrationBuilder.CreateIndex(
        name: "IX_Posts_IsMisleading_IsFalseInformation",
        table: "Posts",
        columns: new[] { "IsMisleading", "IsFalseInformation" });
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(name: "Posts");
  }
}
