using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaratukAdmin.Migrations
{
    public partial class AddPriceBlock1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_PriceBlocks_PriceBlockId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "PriceBlockServices");

            migrationBuilder.RenameIndex(
                name: "IX_Services_PriceBlockId",
                table: "PriceBlockServices",
                newName: "IX_PriceBlockServices_PriceBlockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceBlockServices",
                table: "PriceBlockServices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceBlockServices_PriceBlocks_PriceBlockId",
                table: "PriceBlockServices",
                column: "PriceBlockId",
                principalTable: "PriceBlocks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceBlockServices_PriceBlocks_PriceBlockId",
                table: "PriceBlockServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceBlockServices",
                table: "PriceBlockServices");

            migrationBuilder.RenameTable(
                name: "PriceBlockServices",
                newName: "Services");

            migrationBuilder.RenameIndex(
                name: "IX_PriceBlockServices_PriceBlockId",
                table: "Services",
                newName: "IX_Services_PriceBlockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_PriceBlocks_PriceBlockId",
                table: "Services",
                column: "PriceBlockId",
                principalTable: "PriceBlocks",
                principalColumn: "Id");
        }
    }
}
