using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addtonnesjourney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Tonnes",
                table: "WasteJourney",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tonnes",
                table: "WasteJourney");
        }
    }
}
