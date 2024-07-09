using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IVYModa.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "Locations");
        }
    }
}
