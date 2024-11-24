using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityAdmin.Migrations
{
    /// <inheritdoc />
    public partial class newPermissionColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Permissionname",
                table: "Permission",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Permission",
                newName: "Permissionname");
        }
    }
}
