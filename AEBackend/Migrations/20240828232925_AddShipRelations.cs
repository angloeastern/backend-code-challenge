using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddShipRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShip_AspNetUsers_UserId",
                table: "UserShip");

            migrationBuilder.DropForeignKey(
                name: "FK_UserShip_Ship_ShipId",
                table: "UserShip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShip",
                table: "UserShip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ship",
                table: "Ship");

            migrationBuilder.RenameTable(
                name: "UserShip",
                newName: "UserShips");

            migrationBuilder.RenameTable(
                name: "Ship",
                newName: "Ships");

            migrationBuilder.RenameIndex(
                name: "IX_UserShip_ShipId",
                table: "UserShips",
                newName: "IX_UserShips_ShipId");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShips",
                table: "UserShips",
                columns: new[] { "UserId", "ShipId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ships",
                table: "Ships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShips_AspNetUsers_UserId",
                table: "UserShips",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShips_Ships_ShipId",
                table: "UserShips",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShips_AspNetUsers_UserId",
                table: "UserShips");

            migrationBuilder.DropForeignKey(
                name: "FK_UserShips_Ships_ShipId",
                table: "UserShips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShips",
                table: "UserShips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ships",
                table: "Ships");

            migrationBuilder.RenameTable(
                name: "UserShips",
                newName: "UserShip");

            migrationBuilder.RenameTable(
                name: "Ships",
                newName: "Ship");

            migrationBuilder.RenameIndex(
                name: "IX_UserShips_ShipId",
                table: "UserShip",
                newName: "IX_UserShip_ShipId");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShip",
                table: "UserShip",
                columns: new[] { "UserId", "ShipId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ship",
                table: "Ship",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShip_AspNetUsers_UserId",
                table: "UserShip",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserShip_Ship_ShipId",
                table: "UserShip",
                column: "ShipId",
                principalTable: "Ship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
