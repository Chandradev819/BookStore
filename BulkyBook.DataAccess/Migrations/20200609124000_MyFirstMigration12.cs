using Microsoft.EntityFrameworkCore.Migrations;

namespace BulkyBook.DataAccess.Migrations
{
    public partial class MyFirstMigration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImgs_Products_ProdId",
                table: "ProductImgs");

            migrationBuilder.AlterColumn<int>(
                name: "ProdId",
                table: "ProductImgs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImgs_Products_ProdId",
                table: "ProductImgs",
                column: "ProdId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImgs_Products_ProdId",
                table: "ProductImgs");

            migrationBuilder.AlterColumn<int>(
                name: "ProdId",
                table: "ProductImgs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImgs_Products_ProdId",
                table: "ProductImgs",
                column: "ProdId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
