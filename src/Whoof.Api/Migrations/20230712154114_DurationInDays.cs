using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whoof.Api.Migrations
{
    /// <inheritdoc />
    public partial class DurationInDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Vaccines",
                type: "integer",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Vaccines",
                type: "interval",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
