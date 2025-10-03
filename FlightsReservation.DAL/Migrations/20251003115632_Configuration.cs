using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightsReservation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Flights_FlightId",
                table: "Seat");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Passengers_PassengerId",
                table: "Seat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seat",
                table: "Seat");

            migrationBuilder.DropIndex(
                name: "IX_Seat_PassengerId",
                table: "Seat");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "Seat");

            migrationBuilder.RenameTable(
                name: "Seat",
                newName: "Seats");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_FlightId",
                table: "Seats",
                newName: "IX_Seats_FlightId");

            migrationBuilder.AlterColumn<string>(
                name: "ReservationNumber",
                table: "Reservations",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Passengers",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Passengers",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Passengers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Passengers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Passengers",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Passengers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FlightNumber",
                table: "Flights",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Departure",
                table: "Flights",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Arrival",
                table: "Flights",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AirplaneType",
                table: "Flights",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SeatNumber",
                table: "Seats",
                type: "character varying(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers",
                column: "SeatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Seats_SeatId",
                table: "Passengers",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Flights_FlightId",
                table: "Seats",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Seats_SeatId",
                table: "Passengers");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Flights_FlightId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Passengers");

            migrationBuilder.RenameTable(
                name: "Seats",
                newName: "Seat");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_FlightId",
                table: "Seat",
                newName: "IX_Seat_FlightId");

            migrationBuilder.AlterColumn<string>(
                name: "ReservationNumber",
                table: "Reservations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Passengers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Passengers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Passengers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Passengers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Passengers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "Passengers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FlightNumber",
                table: "Flights",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Departure",
                table: "Flights",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Arrival",
                table: "Flights",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "AirplaneType",
                table: "Flights",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "SeatNumber",
                table: "Seat",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4);

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "Seat",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seat",
                table: "Seat",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_PassengerId",
                table: "Seat",
                column: "PassengerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Flights_FlightId",
                table: "Seat",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Passengers_PassengerId",
                table: "Seat",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");
        }
    }
}
