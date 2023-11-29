using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWasteReferenceNumberToJourney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "WasteJourney",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "WasteJourney");
        }
    }
}
