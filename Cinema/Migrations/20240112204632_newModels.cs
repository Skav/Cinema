using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class newModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<string>(type: "text", nullable: false),
                    discount = table.Column<int>(type: "integer", nullable: false),
                    discountType = table.Column<string>(type: "text", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    expDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.id);
                    table.ForeignKey(
                        name: "FK_COUPONS_USERS",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoyalityPointsModels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<string>(type: "text", nullable: false),
                    amountOfPoints = table.Column<int>(type: "integer", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyalityPointsModels", x => x.id);
                    table.ForeignKey(
                        name: "FK_POINTS_USERS",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    genre = table.Column<string>(type: "text", nullable: false),
                    available = table.Column<bool>(type: "boolean", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roomNo = table.Column<int>(type: "integer", nullable: false),
                    rows = table.Column<int>(type: "integer", nullable: false),
                    columns = table.Column<int>(type: "integer", nullable: false),
                    seatsInRow = table.Column<int>(type: "integer", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewsModel",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    movieId = table.Column<int>(type: "integer", nullable: false),
                    userId = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewsModel", x => x.id);
                    table.ForeignKey(
                        name: "FK_REVIEWS_MOVIES",
                        column: x => x.movieId,
                        principalTable: "Movies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_REVIEWS_USERS",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieShowModels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roomId = table.Column<int>(type: "integer", nullable: false),
                    movieId = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hour = table.Column<string>(type: "text", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShowModels", x => x.id);
                    table.ForeignKey(
                        name: "FK_MOVIESHOW_MOVIE",
                        column: x => x.movieId,
                        principalTable: "Movies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_MOVIESHOW_ROOM",
                        column: x => x.roomId,
                        principalTable: "Rooms",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<string>(type: "text", nullable: false),
                    movieShowId = table.Column<int>(type: "integer", nullable: false),
                    seatRow = table.Column<int>(type: "integer", nullable: false),
                    seatColumn = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    isPaid = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    claimDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_RESERVATIONS_MOVIESHOW",
                        column: x => x.movieShowId,
                        principalTable: "MovieShowModels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_RESERVATION_USER",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_userId",
                table: "Coupons",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyalityPointsModels_userId",
                table: "LoyalityPointsModels",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowModels_movieId",
                table: "MovieShowModels",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowModels_roomId",
                table: "MovieShowModels",
                column: "roomId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_movieShowId",
                table: "Reservations",
                column: "movieShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_userId",
                table: "Reservations",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsModel_movieId",
                table: "ReviewsModel",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsModel_userId",
                table: "ReviewsModel",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "LoyalityPointsModels");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "ReviewsModel");

            migrationBuilder.DropTable(
                name: "MovieShowModels");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
