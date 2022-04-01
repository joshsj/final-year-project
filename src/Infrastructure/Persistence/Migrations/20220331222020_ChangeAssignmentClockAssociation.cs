using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RendezVous.Infrastructure.Persistence.Migrations
{
    public partial class ChangeAssignmentClockAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "Distance");

            migrationBuilder.DropColumn(
                name: "ActualAt",
                table: "Clock");

            migrationBuilder.RenameColumn(
                name: "ExpectedAt",
                table: "Clock",
                newName: "At");

            migrationBuilder.AddColumn<double>(
                name: "Coordinates_Latitude",
                table: "Location",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Coordinates_Longitude",
                table: "Location",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Radius_Meters",
                table: "Location",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Coordinates_Latitude",
                table: "Clock",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Coordinates_Longitude",
                table: "Clock",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Assignment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates_Latitude",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Coordinates_Longitude",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Radius_Meters",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Coordinates_Latitude",
                table: "Clock");

            migrationBuilder.DropColumn(
                name: "Coordinates_Longitude",
                table: "Clock");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Assignment");

            migrationBuilder.RenameColumn(
                name: "At",
                table: "Clock",
                newName: "ExpectedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualAt",
                table: "Clock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Coordinates_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Distance",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Meters = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distance", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Distance_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
