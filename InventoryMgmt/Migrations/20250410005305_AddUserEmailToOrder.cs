using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryMgmt.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEmailToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Orders");
        }
    }
}
