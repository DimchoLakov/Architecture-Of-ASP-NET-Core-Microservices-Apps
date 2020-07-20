using Microsoft.EntityFrameworkCore.Migrations;

namespace MyOnlineShop.ShoppingCart.Data.Migrations
{
    public partial class AddIsArchived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "ShoppingCartItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "ShoppingCartItems");
        }
    }
}
