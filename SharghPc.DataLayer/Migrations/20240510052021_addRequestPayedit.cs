using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharghPc.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addRequestPayedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_RequestPays_RequestPayId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_RequestPayId",
                table: "Carts");

            migrationBuilder.AddColumn<long>(
                name: "CartId",
                table: "RequestPays",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RequestPays_CartId",
                table: "RequestPays",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPays_Carts_CartId",
                table: "RequestPays",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestPays_Carts_CartId",
                table: "RequestPays");

            migrationBuilder.DropIndex(
                name: "IX_RequestPays_CartId",
                table: "RequestPays");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "RequestPays");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_RequestPayId",
                table: "Carts",
                column: "RequestPayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_RequestPays_RequestPayId",
                table: "Carts",
                column: "RequestPayId",
                principalTable: "RequestPays",
                principalColumn: "Id");
        }
    }
}
