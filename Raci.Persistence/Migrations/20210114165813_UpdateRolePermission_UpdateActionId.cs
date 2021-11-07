using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Raci.Persistence.Migrations
{
    public partial class UpdateRolePermission_UpdateActionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "RolePermissions");

            migrationBuilder.AddColumn<long>(
                name: "ActionId2",
                table: "RolePermissions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionId2",
                table: "RolePermissions");

            migrationBuilder.AddColumn<Guid>(
                name: "ActionId",
                table: "RolePermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
