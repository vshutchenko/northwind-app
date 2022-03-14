using System;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Blog article product.
    /// </summary>
    public class BlogArticleProduct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleProduct"/> class.
        /// </summary>
        /// <param name="articleId">A blog article id.</param>
        /// <param name="productId">A product id.</param>
        public BlogArticleProduct(int articleId, int productId)
        {
            this.ArticleId = articleId;
            this.ProductId = productId;
        }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets article id.
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// Gets or sets product id.
        /// </summary>
        public int ProductId { get; set; }
    }
}
