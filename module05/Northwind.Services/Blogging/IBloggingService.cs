using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents a management service for blogs.
    /// </summary>
    public interface IBloggingService
    {
        /// <summary>
        /// Deletes an existed blog article.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <returns>True if a blog article is destroyed; otherwise false.</returns>
        Task<bool> DeleteBlogArticleAsync(int articleId);

        /// <summary>
        /// Gets a collection of blog articles using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="BlogArticle"/>.</returns>
        IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync(int offset, int limit);

        /// <summary>
        /// Gets a blog article with specified identifier.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <returns>Returns blog article or null if blog article does not exist.</returns>
        Task<BlogArticle> GetBlogArticleAsync(int articleId);

        /// <summary>
        /// Creates a new blog article.
        /// </summary>
        /// <param name="article">A <see cref="BlogArticle"/> to create.</param>
        /// <returns>An identifier of a created blog article.</returns>
        Task<int> CreateBlogArticleAsync(BlogArticle article);

        /// <summary>
        /// Updates a blog article.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="article">A <see cref="BlogArticle"/>.</param>
        /// <returns>True if a blog article is updated; otherwise false.</returns>
        Task<bool> UpdateBlogArticleAsync(int articleId, BlogArticle article);

        /// <summary>
        /// Gets a blog article related products.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <returns>Returns blog article or null if blog article does not exist.</returns>
        IAsyncEnumerable<BlogArticleProduct> GetRelatedProductsAsync(int articleId);

        /// <summary>
        /// Creates related product for blog article.
        /// </summary>
        /// <param name="articleProduct">A <see cref="BlogArticleProduct"/> to create.</param>
        /// <returns>An identifier of a created blog article product.</returns>
        Task<int> CreateRelatedProductAsync(BlogArticleProduct articleProduct);

        /// <summary>
        /// Deletes related product for blog article.
        /// </summary>
        /// <param name="articleId">Article id.</param>
        /// <param name="productId">Product id.</param>
        /// <returns>An identifier of a blog article product to delete.</returns>
        Task<bool> DeleteRelatedProductAsync(int articleId, int productId);

        /// <summary>
        /// Gets article comments.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>Comments collection.</returns>
        public IAsyncEnumerable<BlogComment> GetBlogArticleCommentsAsync(int articleId, int offset, int limit);

        /// <summary>
        /// Creates comment for article.
        /// </summary>
        /// <param name="comment">Comment to create.</param>
        /// <returns>An identifier of a created article comment.</returns>
        public Task<int> CreateBlogArticleCommentAsync(BlogComment comment);

        /// <summary>
        /// Updates article comment.
        /// </summary>
        /// <param name="commentId">A blog article comment identifier.</param>
        /// <param name="comment">Comment to update.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        public Task<bool> UpdateBlogArticleCommentAsync(int commentId, BlogComment comment);

        /// <summary>
        /// Deletes article comment.
        /// </summary>
        /// <param name="commentId">Id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        public Task<bool> DeleteBlogArticleCommentAsync(int commentId);
    }
}
