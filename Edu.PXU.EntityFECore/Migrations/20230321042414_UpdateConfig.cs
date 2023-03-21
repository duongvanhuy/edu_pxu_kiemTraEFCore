using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edu.PXU.EntityFECore.Migrations
{
    public partial class UpdateConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Images_IdImage",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "ProductImages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ImageId",
                table: "ProductImages",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Images_IdImage",
                table: "ProductImages",
                column: "IdImage",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Images_ImageId",
                table: "ProductImages",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Images_IdImage",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Images_ImageId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ImageId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Images_IdImage",
                table: "ProductImages",
                column: "IdImage",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
