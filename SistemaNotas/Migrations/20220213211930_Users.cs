using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaNotas.Migrations
{
    public partial class Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Notas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_Username",
                table: "Notas",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Users_Username",
                table: "Notas",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Users_Username",
                table: "Notas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Notas_Username",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Notas");
        }
    }
}
