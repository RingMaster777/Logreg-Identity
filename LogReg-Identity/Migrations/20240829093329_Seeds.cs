using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LogReg_Identity.Migrations
{
    /// <inheritdoc />
    public partial class Seeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RpId", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1, "452bf40a-f19a-4cac-ac26-2bd78679ed41" },
                    { 2, 2, "452bf40a-f19a-4cac-ac26-2bd78679ed41" },
                    { 3, 3, "452bf40a-f19a-4cac-ac26-2bd78679ed41" },
                    { 4, 4, "452bf40a-f19a-4cac-ac26-2bd78679ed41" },
                    { 5, 2, "4cccab98-a3e5-4dfc-958e-bf7b6a8ad9cc" },
                    { 6, 1, "4cccab98-a3e5-4dfc-958e-bf7b6a8ad9cc" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RpId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RpId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RpId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RpId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RpId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RpId",
                keyValue: 6);
        }
    }
}
