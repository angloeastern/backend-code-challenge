using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeShipLongToLongi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Long",
                table: "Ships",
                newName: "Longi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longi",
                table: "Ships",
                newName: "Long");
        }
    }
}
