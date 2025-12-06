using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_PRJ_F.Migrations
{
    /// <inheritdoc />
    public partial class mmovieF1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Movies");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
