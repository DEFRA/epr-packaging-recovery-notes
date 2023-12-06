using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDoneWasteToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DoneWaste",
                table: "WasteJourney",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoneWaste",
                table: "WasteJourney");
        }
    }
}
