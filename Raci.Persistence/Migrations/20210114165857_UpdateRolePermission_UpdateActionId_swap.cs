using Microsoft.EntityFrameworkCore.Migrations;

namespace Raci.Persistence.Migrations
{
    public partial class UpdateRolePermission_UpdateActionId_swap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionId2",
                table: "RolePermissions",
                newName: "ActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionId",
                table: "RolePermissions",
                newName: "ActionId2");
        }
    }
}
