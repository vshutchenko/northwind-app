using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    public partial class ChangeAuthorIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "author_id",
                table: "comments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "author_id",
                table: "comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
