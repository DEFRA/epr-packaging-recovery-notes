using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waste.API.Migrations
{
    /// <inheritdoc />
    public partial class WasteTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using StreamReader reader = new StreamReader(File.OpenRead(".\\Migrations\\LoadSQL\\waste-types.sql"));

            migrationBuilder.DropTable(
                name: "DefaultTable");

            migrationBuilder.CreateTable(
                name: "WasteType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WasteSubType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adjustment = table.Column<double>(type: "float", nullable: true),
                    WasteTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteSubType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WasteSubType_WasteType_WasteTypeId",
                        column: x => x.WasteTypeId,
                        principalTable: "WasteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WasteJourney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    Adjustment = table.Column<double>(type: "float", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
                    BaledWithWire = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: true),
                    WasteSubTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteJourney", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WasteJourney_WasteSubType_WasteSubTypeId",
                        column: x => x.WasteSubTypeId,
                        principalTable: "WasteSubType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WasteJourney_WasteSubTypeId",
                table: "WasteJourney",
                column: "WasteSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteSubType_WasteTypeId",
                table: "WasteSubType",
                column: "WasteTypeId");
            
            migrationBuilder.Sql(reader.ReadToEnd());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WasteJourney");

            migrationBuilder.DropTable(
                name: "WasteSubType");

            migrationBuilder.DropTable(
                name: "WasteType");

            migrationBuilder.CreateTable(
                name: "DefaultTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultTable", x => x.Id);
                });
        }
    }
}
