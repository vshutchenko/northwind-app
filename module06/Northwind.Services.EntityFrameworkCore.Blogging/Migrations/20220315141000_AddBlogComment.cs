using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    public partial class AddBlogComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(type: "ntext", nullable: false),
                    posted = table.Column<DateTime>(type: "datetime", nullable: false),
                    author_id = table.Column<int>(nullable: false),
                    article_id = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");
        }
    }
}
