using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Raci.Persistence.Migrations
{
    public partial class Update_RaciAccount_EnumAsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "RaciAccounts");

            migrationBuilder.DropColumn(
                name: "LockoutEndDateUtc",
                table: "RaciAccounts");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "RaciAccounts",
                newName: "AuditStatus");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "RaciAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "RaciAccounts");

            migrationBuilder.RenameColumn(
                name: "AuditStatus",
                table: "RaciAccounts",
                newName: "StatusId");

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "RaciAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEndDateUtc",
                table: "RaciAccounts",
                type: "datetime2",
                nullable: true);
        }
    }
}
