using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteIdforReprocessorWaste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "WasteJourney",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "WasteJourney");
        }
    }
}
