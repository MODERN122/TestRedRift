using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiceGameApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatePostgres2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurnNumber",
                table: "Turns",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnNumber",
                table: "Turns");
        }
    }
}
