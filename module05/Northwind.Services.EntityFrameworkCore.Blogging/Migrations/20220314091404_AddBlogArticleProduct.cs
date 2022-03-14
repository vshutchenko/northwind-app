using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    public partial class AddBlogArticleProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles_products",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_id = table.Column<int>(nullable: false),
                    product_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles_products", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles_products");
        }
    }
}
