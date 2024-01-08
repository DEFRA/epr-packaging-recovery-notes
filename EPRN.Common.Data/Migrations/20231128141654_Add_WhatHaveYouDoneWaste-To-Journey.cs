using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_WhatHaveYouDoneWasteToJourney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhatHaveYouDoneWaste",
                table: "WasteJourney",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatHaveYouDoneWaste",
                table: "WasteJourney");
        }
    }
}
