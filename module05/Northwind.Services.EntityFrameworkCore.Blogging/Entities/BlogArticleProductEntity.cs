using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    /// <summary>
    /// Blog article entity.
    /// </summary>
    [Table("articles_products")]
    public class BlogArticleProductEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleProductEntity"/> class.
        /// </summary>
        public BlogArticleProductEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleProductEntity"/> class.
        /// </summary>
        /// <param name="articleId">A blog article id.</param>
        /// <param name="productId">A product id.</param>
        public BlogArticleProductEntity(int articleId, int productId)
        {
            this.ArticleId = articleId;
            this.ProductId = productId;
        }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets article id.
        /// </summary>
        [Column("article_id")]
        public int ArticleId { get; set; }

        /// <summary>
        /// Gets or sets product id.
        /// </summary>
        [Column("product_id")]
        public int ProductId { get; set; }
    }
}
