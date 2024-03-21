using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrackerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBgModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_AspNetUsers_AssignedToId",
                table: "Bugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_AspNetUsers_ReportedById",
                table: "Bugs");

            migrationBuilder.DropIndex(
                name: "IX_Bugs_AssignedToId",
                table: "Bugs");

            migrationBuilder.DropIndex(
                name: "IX_Bugs_ReportedById",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "ReportedById",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "ReportedByUserId",
                table: "Bugs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "Bugs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserId",
                table: "Bugs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReportedById",
                table: "Bugs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportedByUserId",
                table: "Bugs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_AssignedToId",
                table: "Bugs",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_ReportedById",
                table: "Bugs",
                column: "ReportedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_AspNetUsers_AssignedToId",
                table: "Bugs",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_AspNetUsers_ReportedById",
                table: "Bugs",
                column: "ReportedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
