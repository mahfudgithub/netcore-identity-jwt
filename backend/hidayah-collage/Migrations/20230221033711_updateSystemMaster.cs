using Microsoft.EntityFrameworkCore.Migrations;

namespace hidayah_collage.Migrations
{
    public partial class updateSystemMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_System",
                table: "System");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "System");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "System",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "System",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_System",
                table: "System",
                columns: new[] { "Type", "Code" });

            migrationBuilder.CreateTable(
                name: "messageListNotMappeds",
                columns: table => new
                {
                    SEQ = table.Column<long>(nullable: false),
                    MSG_CD = table.Column<string>(nullable: true),
                    MSG_TEXT = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messageListNotMappeds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_System",
                table: "System");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "System",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "System",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "System",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_System",
                table: "System",
                column: "Id");
        }
    }
}
