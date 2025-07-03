using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whispers.Chat.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddUser : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
        name: "Users",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
          Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
          PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
          LikedPostIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
          IsAnonymous = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
          DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
          DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
          CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Users", x => x.Id);
        });

    // Create indexes for better query performance
    migrationBuilder.CreateIndex(
        name: "IX_Users_Username",
        table: "Users",
        column: "Username",
        unique: true);

    migrationBuilder.CreateIndex(
        name: "IX_Users_Email",
        table: "Users",
        column: "Email",
        unique: true);

    migrationBuilder.CreateIndex(
        name: "IX_Users_IsAnonymous",
        table: "Users",
        column: "IsAnonymous");

    migrationBuilder.CreateIndex(
        name: "IX_Users_DateCreated",
        table: "Users",
        column: "DateCreated");
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(name: "Users");
  }
}
