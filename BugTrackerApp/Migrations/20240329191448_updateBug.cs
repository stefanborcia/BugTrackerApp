using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class updateBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowInBugList",
                table: "Bugs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowInBugList",
                table: "Bugs");
        }
    }
}
