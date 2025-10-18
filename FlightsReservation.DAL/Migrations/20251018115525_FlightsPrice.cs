using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightsReservation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FlightsPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Lock",
                table: "Seats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "TIMESTAMP '1970-01-01 00:00:00'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "1970-01-01 00:00:00'::timestamp");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Flights",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Flights",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Flights");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lock",
                table: "Seats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "1970-01-01 00:00:00'::timestamp",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "TIMESTAMP '1970-01-01 00:00:00'");
        }
    }
}
