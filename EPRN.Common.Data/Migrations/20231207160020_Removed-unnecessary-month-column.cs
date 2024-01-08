using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class Removedunnecessarymonthcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthReceived",
                table: "WasteJourney");

            migrationBuilder.RenameColumn(
                name: "MonthSent",
                table: "WasteJourney",
                newName: "Month");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Month",
                table: "WasteJourney",
                newName: "MonthSent");

            migrationBuilder.AddColumn<int>(
                name: "MonthReceived",
                table: "WasteJourney",
                type: "int",
                nullable: true);
        }
    }
}
