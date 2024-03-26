using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SolvedBugs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SolvedBugs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
