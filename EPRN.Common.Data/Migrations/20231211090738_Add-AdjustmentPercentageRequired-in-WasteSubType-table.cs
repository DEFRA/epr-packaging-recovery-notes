using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdjustmentPercentageRequiredinWasteSubTypetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdjustmentPercentageRequired",
                table: "WasteSubType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"
                UPDATE WasteSubType
                SET AdjustmentPercentageRequired = 1
                WHERE [Name] = 'Other'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentPercentageRequired",
                table: "WasteSubType");
        }
    }
}
