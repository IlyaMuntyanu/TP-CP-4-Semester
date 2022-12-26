using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPCP5Semester.Migrations
{
    /// <inheritdoc />
    public partial class adddeletedfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Tours",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Tours");
        }
    }
}
