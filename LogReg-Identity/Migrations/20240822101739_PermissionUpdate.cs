using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogReg_Identity.Migrations
{
    /// <inheritdoc />
    public partial class PermissionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 1,
                column: "PermissionName",
                value: "POST");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 2,
                column: "PermissionName",
                value: "GET");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 1,
                column: "PermissionName",
                value: "GET");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 2,
                column: "PermissionName",
                value: "POST");
        }
    }
}
