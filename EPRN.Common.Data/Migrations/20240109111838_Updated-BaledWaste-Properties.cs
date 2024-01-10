using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBaledWasteProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeductionAmount",
                table: "WasteJourney",
                newName: "BaledWithWireDeductionPercentage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaledWithWireDeductionPercentage",
                table: "WasteJourney",
                newName: "DeductionAmount");
        }
    }
}
