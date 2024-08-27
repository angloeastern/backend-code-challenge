using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddKnot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Velocity_UnitName",
                table: "Ship",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Velocity_Value",
                table: "Ship",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Velocity_UnitName",
                table: "Ship");

            migrationBuilder.DropColumn(
                name: "Velocity_Value",
                table: "Ship");
        }
    }
}
