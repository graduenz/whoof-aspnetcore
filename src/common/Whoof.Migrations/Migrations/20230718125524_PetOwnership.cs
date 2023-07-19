using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whoof.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class PetOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Vaccines",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Vaccines",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PetVaccinations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "PetVaccinations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Pets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Pets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Pets",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Vaccines");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Vaccines");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PetVaccinations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PetVaccinations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Pets");
        }
    }
}
