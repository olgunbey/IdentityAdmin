using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityAdmin.Migrations
{
    /// <inheritdoc />
    public partial class permissioncolumnadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "ClientRole");

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "UserRole",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "UserRole");

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "ClientRole",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
