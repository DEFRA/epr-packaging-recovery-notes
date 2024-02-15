using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserReferenceToPRNTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DecemberWaste",
                table: "PRN",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserReferenceId",
                table: "PRN",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecemberWaste",
                table: "PRN");

            migrationBuilder.DropColumn(
                name: "UserReferenceId",
                table: "PRN");
        }
    }
}
