using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IVYModa.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Email_PhoneNumber",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "Index_Email_PhoneNumber",
                table: "Users",
                columns: new[] { "Email", "PhoneNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Email_PhoneNumber",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.CreateIndex(
                name: "Index_Email_PhoneNumber",
                table: "Users",
                columns: new[] { "Email", "PhoneNumber" },
                unique: true,
                filter: "[Email] IS NOT NULL");
        }
    }
}
