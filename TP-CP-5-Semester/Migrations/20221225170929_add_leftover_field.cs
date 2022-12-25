using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPCP5Semester.Migrations
{
    /// <inheritdoc />
    public partial class addleftoverfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Leftover",
                table: "Tours",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leftover",
                table: "Tours");
        }
    }
}
