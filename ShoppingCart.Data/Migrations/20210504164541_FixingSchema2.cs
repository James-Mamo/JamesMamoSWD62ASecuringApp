using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCart.Data.Migrations
{
    public partial class FixingSchema2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Teacher_TeacherId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Students_TeacherId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "TeacherEmail",
                table: "Students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherEmail",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_TeacherId",
                table: "Students",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Teacher_TeacherId",
                table: "Students",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
