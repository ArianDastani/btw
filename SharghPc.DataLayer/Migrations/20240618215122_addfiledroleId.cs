using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharghPc.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addfiledroleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RolesId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "Users");
        }
    }
}
