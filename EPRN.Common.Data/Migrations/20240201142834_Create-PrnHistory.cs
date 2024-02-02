using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPRN.Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class CreatePrnHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PRN");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PRN");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "PRN");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "PRN");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PRN");

            migrationBuilder.DropColumn(
                name: "StatusReason",
                table: "PRN");

            migrationBuilder.CreateTable(
                name: "PRNHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrnId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRNHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRNHistory_PRN_PrnId",
                        column: x => x.PrnId,
                        principalTable: "PRN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRNHistory_PrnId",
                table: "PRNHistory",
                column: "PrnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRNHistory");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PRN",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PRN",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "PRN",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "PRN",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PRN",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusReason",
                table: "PRN",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
