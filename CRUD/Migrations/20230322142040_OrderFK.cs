using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    public partial class OrderFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Order_ProviderId",
                table: "Order",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Provider_ProviderId",
                table: "Order",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Provider_ProviderId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ProviderId",
                table: "Order");
        }
    }
}
