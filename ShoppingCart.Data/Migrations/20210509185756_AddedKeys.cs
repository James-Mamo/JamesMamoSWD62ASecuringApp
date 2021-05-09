using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCart.Data.Migrations
{
    public partial class AddedKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TeacherEmail",
                table: "Students",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "privateKey",
                table: "Students",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "publicKey",
                table: "Students",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "privateKey",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "publicKey",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherEmail",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
