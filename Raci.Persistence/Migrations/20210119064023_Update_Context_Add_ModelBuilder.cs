using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Raci.Persistence.Migrations
{
    public partial class Update_Context_Add_ModelBuilder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Items_ItemId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RaciAccounts_RaciAccountId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RaciAccounts_Shops_ShopId",
                table: "RaciAccounts");

            migrationBuilder.DropIndex(
                name: "IX_RaciAccounts_ShopId",
                table: "RaciAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AccountId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RaciAccountId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ItemId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "RaciAccounts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RaciAccountId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderDetails");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "UserAssignments",
                newName: "UserAssignments",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Shops",
                newName: "Shops",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "RolePermissions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "RaciAccounts",
                newName: "RaciAccounts",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Orders",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "OrderDetails",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Modules",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "Items",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Functions",
                newName: "Functions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Actions",
                newName: "Actions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Accounts",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "AuditStatus",
                schema: "dbo",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                schema: "dbo",
                table: "RaciAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "RaciAccounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                schema: "dbo",
                table: "RaciAccounts",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "vi",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "dbo",
                table: "RaciAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AuditStatus",
                schema: "dbo",
                table: "RaciAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "OrderStatus",
                schema: "dbo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                schema: "dbo",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AuditStatus",
                schema: "dbo",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Functions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Actions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "vi",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CountryCallingCode",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditStatus",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "dbo",
                columns: table => new
                {
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TablePk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaciAccounts_PhoneNumber",
                schema: "dbo",
                table: "RaciAccounts",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RaciAccounts_ShopGuid",
                schema: "dbo",
                table: "RaciAccounts",
                column: "ShopGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountGuid",
                schema: "dbo",
                table: "Orders",
                column: "AccountGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RaciAccountGuid",
                schema: "dbo",
                table: "Orders",
                column: "RaciAccountGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopGuid",
                schema: "dbo",
                table: "Orders",
                column: "ShopGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ItemGuid",
                schema: "dbo",
                table: "OrderDetails",
                column: "ItemGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderGuid",
                schema: "dbo",
                table: "OrderDetails",
                column: "OrderGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PhoneNumber_CountryCallingCode",
                schema: "dbo",
                table: "Accounts",
                columns: new[] { "PhoneNumber", "CountryCallingCode" },
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL AND [CountryCallingCode] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Items_ItemGuid",
                schema: "dbo",
                table: "OrderDetails",
                column: "ItemGuid",
                principalSchema: "dbo",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderGuid",
                schema: "dbo",
                table: "OrderDetails",
                column: "OrderGuid",
                principalSchema: "dbo",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_AccountGuid",
                schema: "dbo",
                table: "Orders",
                column: "AccountGuid",
                principalSchema: "dbo",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RaciAccounts_RaciAccountGuid",
                schema: "dbo",
                table: "Orders",
                column: "RaciAccountGuid",
                principalSchema: "dbo",
                principalTable: "RaciAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shops_ShopGuid",
                schema: "dbo",
                table: "Orders",
                column: "ShopGuid",
                principalSchema: "dbo",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RaciAccounts_Shops_ShopGuid",
                schema: "dbo",
                table: "RaciAccounts",
                column: "ShopGuid",
                principalSchema: "dbo",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Items_ItemGuid",
                schema: "dbo",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderGuid",
                schema: "dbo",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_AccountGuid",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RaciAccounts_RaciAccountGuid",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shops_ShopGuid",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RaciAccounts_Shops_ShopGuid",
                schema: "dbo",
                table: "RaciAccounts");

            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_RaciAccounts_PhoneNumber",
                schema: "dbo",
                table: "RaciAccounts");

            migrationBuilder.DropIndex(
                name: "IX_RaciAccounts_ShopGuid",
                schema: "dbo",
                table: "RaciAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AccountGuid",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RaciAccountGuid",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopGuid",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ItemGuid",
                schema: "dbo",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderGuid",
                schema: "dbo",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_PhoneNumber_CountryCallingCode",
                schema: "dbo",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "UserAssignments",
                schema: "dbo",
                newName: "UserAssignments");

            migrationBuilder.RenameTable(
                name: "Shops",
                schema: "dbo",
                newName: "Shops");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "dbo",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                schema: "dbo",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "RaciAccounts",
                schema: "dbo",
                newName: "RaciAccounts");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "dbo",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                schema: "dbo",
                newName: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "Modules",
                schema: "dbo",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "dbo",
                newName: "Items");

            migrationBuilder.RenameTable(
                name: "Functions",
                schema: "dbo",
                newName: "Functions");

            migrationBuilder.RenameTable(
                name: "Actions",
                schema: "dbo",
                newName: "Actions");

            migrationBuilder.RenameTable(
                name: "Accounts",
                schema: "dbo",
                newName: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "AuditStatus",
                table: "Shops",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Roles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "RaciAccounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "RaciAccounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                table: "RaciAccounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "vi");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "RaciAccounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AuditStatus",
                table: "RaciAccounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
                table: "RaciAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RaciAccountId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Modules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "Items",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AuditStatus",
                table: "Items",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Functions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Actions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserType",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "vi");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CountryCallingCode",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuditStatus",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_RaciAccounts_ShopId",
                table: "RaciAccounts",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountId",
                table: "Orders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RaciAccountId",
                table: "Orders",
                column: "RaciAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopId",
                table: "Orders",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ItemId",
                table: "OrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Items_ItemId",
                table: "OrderDetails",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RaciAccounts_RaciAccountId",
                table: "Orders",
                column: "RaciAccountId",
                principalTable: "RaciAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shops_ShopId",
                table: "Orders",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RaciAccounts_Shops_ShopId",
                table: "RaciAccounts",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
