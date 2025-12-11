using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Busticket.Migrations
{
    /// <inheritdoc />
    public partial class sapo3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Asiento");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "Asiento",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Asiento");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Asiento",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
