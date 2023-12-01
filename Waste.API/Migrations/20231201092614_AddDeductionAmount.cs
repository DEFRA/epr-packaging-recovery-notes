using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDeductionAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DeductionAmount",
                table: "WasteJourney",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeductionAmount",
                table: "WasteJourney");
        }
    }
}
