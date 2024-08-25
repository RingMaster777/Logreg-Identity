using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogReg_Identity.Migrations
{
    /// <inheritdoc />
    public partial class Note : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 2,
                column: "PermissionName",
                value: "Get");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: 2,
                column: "PermissionName",
                value: "Read");
        }
    }
}
