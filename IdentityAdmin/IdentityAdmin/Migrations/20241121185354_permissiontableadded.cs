using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityAdmin.Migrations
{
    /// <inheritdoc />
    public partial class permissiontableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "UserRole");

            migrationBuilder.CreateTable(
                name: "UserRolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRoleId = table.Column<int>(type: "int", nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolePermission_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermission_UserRoleId",
                table: "UserRolePermission",
                column: "UserRoleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRolePermission");

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "UserRole",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
