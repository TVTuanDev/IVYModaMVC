using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IVYModa.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableLoginLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginLog_Users_IdUser",
                table: "LoginLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginLog",
                table: "LoginLog");

            migrationBuilder.RenameTable(
                name: "LoginLog",
                newName: "LoginLogs");

            migrationBuilder.RenameIndex(
                name: "IX_LoginLog_IdUser",
                table: "LoginLogs",
                newName: "IX_LoginLogs_IdUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginLogs",
                table: "LoginLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginLogs_Users_IdUser",
                table: "LoginLogs",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginLogs_Users_IdUser",
                table: "LoginLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginLogs",
                table: "LoginLogs");

            migrationBuilder.RenameTable(
                name: "LoginLogs",
                newName: "LoginLog");

            migrationBuilder.RenameIndex(
                name: "IX_LoginLogs_IdUser",
                table: "LoginLog",
                newName: "IX_LoginLog_IdUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginLog",
                table: "LoginLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginLog_Users_IdUser",
                table: "LoginLog",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
