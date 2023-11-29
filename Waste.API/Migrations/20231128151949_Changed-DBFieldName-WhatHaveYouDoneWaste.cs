using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDBFieldNameWhatHaveYouDoneWaste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhatHaveYouDoneWaste",
                table: "WasteJourney",
                newName: "DoneWaste");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoneWaste",
                table: "WasteJourney",
                newName: "WhatHaveYouDoneWaste");
        }
    }
}
