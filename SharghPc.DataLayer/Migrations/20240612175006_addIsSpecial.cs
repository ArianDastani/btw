using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharghPc.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addIsSpecial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSpecial",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSpecial",
                table: "Products");
        }
    }
}
