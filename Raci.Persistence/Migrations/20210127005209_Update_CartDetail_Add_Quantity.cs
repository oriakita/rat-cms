using Microsoft.EntityFrameworkCore.Migrations;

namespace Raci.Persistence.Migrations
{
    public partial class Update_CartDetail_Add_Quantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "dbo",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "dbo",
                table: "CartDetails");
        }
    }
}
