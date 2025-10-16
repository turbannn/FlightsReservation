using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightsReservation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeatLock_Config : Migration
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
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Lock",
                table: "Seats",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "TIMESTAMP '1970-01-01 00:00:00'");
        }
    }
}
