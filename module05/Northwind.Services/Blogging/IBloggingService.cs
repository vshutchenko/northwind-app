﻿using System.Collections.Generic;
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
    }
}