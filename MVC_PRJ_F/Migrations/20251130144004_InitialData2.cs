using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_PRJ_F.Migrations
{
    /// <inheritdoc />
    public partial class InitialData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CinemaMovies_Movies_MovieId",
                table: "CinemaMovies");

            migrationBuilder.DropTable(
                name: "CategoryMoveCategory");

            migrationBuilder.DropTable(
                name: "MoveCategoryMovie");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CinemaMovies",
                newName: "Time");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "CinemaMovies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MoveCategories_CategoryId",
                table: "MoveCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveCategories_MovieId",
                table: "MoveCategories",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_CinemaMovies_Movies_MovieId",
                table: "CinemaMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveCategories_Categories_CategoryId",
                table: "MoveCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveCategories_Movies_MovieId",
                table: "MoveCategories",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CinemaMovies_Movies_MovieId",
                table: "CinemaMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveCategories_Categories_CategoryId",
                table: "MoveCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveCategories_Movies_MovieId",
                table: "MoveCategories");

            migrationBuilder.DropIndex(
                name: "IX_MoveCategories_CategoryId",
                table: "MoveCategories");

            migrationBuilder.DropIndex(
                name: "IX_MoveCategories_MovieId",
                table: "MoveCategories");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CinemaMovies");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "CinemaMovies",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryMoveCategory",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryMoveCategory", x => new { x.CategoriesId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_CategoryMoveCategory_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryMoveCategory_MoveCategories_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "MoveCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoveCategoryMovie",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveCategoryMovie", x => new { x.CategoriesId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_MoveCategoryMovie_MoveCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "MoveCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveCategoryMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMoveCategory_MoviesId",
                table: "CategoryMoveCategory",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveCategoryMovie_MoviesId",
                table: "MoveCategoryMovie",
                column: "MoviesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CinemaMovies_Movies_MovieId",
                table: "CinemaMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
