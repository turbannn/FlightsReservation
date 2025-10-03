using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlightsReservation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Seats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableSeats",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Flights");

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeatNumber = table.Column<string>(type: "text", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    PassengerId = table.Column<int>(type: "integer", nullable: true),
                    FlightId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seat_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seat_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seat_FlightId",
                table: "Seat",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_PassengerId",
                table: "Seat",
                column: "PassengerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.AddColumn<int>(
                name: "AvailableSeats",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
