using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogReg_Identity.Migrations
{
    /// <inheritdoc />
    public partial class SlightUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 3,
                column: "PermissionName",
                value: "PATCH");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 4,
                column: "PermissionName",
                value: "DELETE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 1,
                column: "PermissionName",
                value: "Create");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 2,
                column: "PermissionName",
                value: "Get");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 3,
                column: "PermissionName",
                value: "Update");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 4,
                column: "PermissionName",
                value: "Delete");
        }
    }
}
