using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedMonthSentandMonthReceived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Month",
                table: "WasteJourney",
                newName: "MonthSent");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "WasteJourney",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "MonthReceived",
                table: "WasteJourney",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthReceived",
                table: "WasteJourney");

            migrationBuilder.RenameColumn(
                name: "MonthSent",
                table: "WasteJourney",
                newName: "Month");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "WasteJourney",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
