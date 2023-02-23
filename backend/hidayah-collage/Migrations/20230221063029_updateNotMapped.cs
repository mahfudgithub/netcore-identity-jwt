using Microsoft.EntityFrameworkCore.Migrations;

namespace hidayah_collage.Migrations
{
    public partial class updateNotMapped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messageListNotMappeds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "messageListNotMappeds",
                columns: table => new
                {
                    MSG_CD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MSG_TEXT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SEQ = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });
        }
    }
}
