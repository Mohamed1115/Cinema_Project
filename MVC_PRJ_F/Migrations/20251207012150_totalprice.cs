using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_PRJ_F.Migrations
{
    /// <inheritdoc />
    public partial class totalprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalCarts",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCarts",
                table: "AspNetUsers");
        }
    }
}
