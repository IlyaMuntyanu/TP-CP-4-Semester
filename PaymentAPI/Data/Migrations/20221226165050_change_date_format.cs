using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedateformat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidThrough",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "ValidThroughMonth",
                table: "Cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValidThroughYear",
                table: "Cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidThroughMonth",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ValidThroughYear",
                table: "Cards");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ValidThrough",
                table: "Cards",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
