using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RendezVous.Infrastructure.Persistence.Migrations
{
    public partial class AddConfirmerConfirmeeToClock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmationToken_Assignment_AssignmentId",
                table: "ConfirmationToken");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "ConfirmationToken",
                newName: "ConfirmerAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmationToken_AssignmentId",
                table: "ConfirmationToken",
                newName: "IX_ConfirmationToken_ConfirmerAssignmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ConfirmationToken",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ConfirmeeAssignmentId",
                table: "ConfirmationToken",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationToken_ConfirmeeAssignmentId",
                table: "ConfirmationToken",
                column: "ConfirmeeAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationToken_Value",
                table: "ConfirmationToken",
                column: "Value",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmationToken_Assignment_ConfirmeeAssignmentId",
                table: "ConfirmationToken",
                column: "ConfirmeeAssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmationToken_Assignment_ConfirmerAssignmentId",
                table: "ConfirmationToken",
                column: "ConfirmerAssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmationToken_Assignment_ConfirmeeAssignmentId",
                table: "ConfirmationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmationToken_Assignment_ConfirmerAssignmentId",
                table: "ConfirmationToken");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmationToken_ConfirmeeAssignmentId",
                table: "ConfirmationToken");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmationToken_Value",
                table: "ConfirmationToken");

            migrationBuilder.DropColumn(
                name: "ConfirmeeAssignmentId",
                table: "ConfirmationToken");

            migrationBuilder.RenameColumn(
                name: "ConfirmerAssignmentId",
                table: "ConfirmationToken",
                newName: "AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmationToken_ConfirmerAssignmentId",
                table: "ConfirmationToken",
                newName: "IX_ConfirmationToken_AssignmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ConfirmationToken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmationToken_Assignment_AssignmentId",
                table: "ConfirmationToken",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
