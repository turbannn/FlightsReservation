﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightsReservation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeatsLock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Lock",
                table: "Seats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lock",
                table: "Seats");
        }
    }
}
