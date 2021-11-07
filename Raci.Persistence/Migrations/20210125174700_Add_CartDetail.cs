using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Raci.Persistence.Migrations
{
    public partial class Add_CartDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "dbo",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_AccountId",
                schema: "dbo",
                table: "CartDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_ItemId",
                schema: "dbo",
                table: "CartDetails",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDetails",
                schema: "dbo");
        }
    }
}
