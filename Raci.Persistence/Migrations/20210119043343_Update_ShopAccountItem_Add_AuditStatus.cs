using Microsoft.EntityFrameworkCore.Migrations;

namespace Raci.Persistence.Migrations
{
    public partial class Update_ShopAccountItem_Add_AuditStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditStatus",
                table: "Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuditStatus",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuditStatus",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditStatus",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "AuditStatus",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AuditStatus",
                table: "Accounts");
        }
    }
}
