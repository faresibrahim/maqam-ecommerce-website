using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicShoppingCartMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderStatusId",
                table: "OrderStatus",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OrderStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusName",
                table: "OrderStatus",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "StatusName",
                table: "OrderStatus");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrderStatus",
                newName: "OrderStatusId");
        }
    }
}
