using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddPort : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Port",
                table: "Port");

            migrationBuilder.RenameTable(
                name: "Port",
                newName: "Ports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ports",
                table: "Ports",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ports",
                table: "Ports");

            migrationBuilder.RenameTable(
                name: "Ports",
                newName: "Port");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Port",
                table: "Port",
                column: "Id");
        }
    }
}
