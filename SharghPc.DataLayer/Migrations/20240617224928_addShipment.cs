using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharghPc.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ShipmentId",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipmentId",
                table: "Orders",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_UserId",
                table: "Shipment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shipment_ShipmentId",
                table: "Orders",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shipment_ShipmentId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShipmentId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "Orders");
        }
    }
}
