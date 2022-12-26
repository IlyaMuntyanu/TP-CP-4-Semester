using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class addunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardNumber",
                table: "Cards",
                column: "CardNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cards_CardNumber",
                table: "Cards");
        }
    }
}
