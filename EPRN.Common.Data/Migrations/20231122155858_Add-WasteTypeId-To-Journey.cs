using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWasteTypeIdToJourney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WasteTypeId",
                table: "WasteJourney",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WasteJourney_WasteTypeId",
                table: "WasteJourney",
                column: "WasteTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WasteJourney_WasteType_WasteTypeId",
                table: "WasteJourney",
                column: "WasteTypeId",
                principalTable: "WasteType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WasteJourney_WasteType_WasteTypeId",
                table: "WasteJourney");

            migrationBuilder.DropIndex(
                name: "IX_WasteJourney_WasteTypeId",
                table: "WasteJourney");

            migrationBuilder.DropColumn(
                name: "WasteTypeId",
                table: "WasteJourney");
        }
    }
}
