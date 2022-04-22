using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    /// <summary>
    /// Represents a blog article management service.
    /// </summary>
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingService"/> class.
        /// </summary>
        /// <param name="context">Data base context.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public BloggingService(BloggingContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateBlogArticleAsync(BlogArticle article)
        {
            article = article ?? throw new ArgumentNullException(nameof(article));

            article.Id = 0;
            var entity = this.mapper.Map<BlogArticleEntity>(article);

            await this.context.BlogArticles.AddAsync(entity);
            await this.context.SaveChangesAsync();

            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<int> CreateBlogArticleCommentAsync(BlogComment comment)
        {
            comment = comment ?? throw new ArgumentNullException(nameof(comment));

            bool articleExists = await this.context.BlogArticles
                .AnyAsync(a => a.Id == comment.ArticleId);

            if (!articleExists)
            {
                return -1;
            }

            comment.Id = 0;
            var entity = this.mapper.Map<BlogCommentEntity>(comment);

            await this.context.BlogComments.AddAsync(entity);
            await this.context.SaveChangesAsync();

            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<int> CreateRelatedProductAsync(BlogArticleProduct articleProduct)
        {
            articleProduct = articleProduct ?? throw new ArgumentNullException(nameof(articleProduct));

            bool exists = await this.context.BlogArticleProducts
                .AnyAsync(p => p.ArticleId == articleProduct.ArticleId && p.ProductId == articleProduct.ProductId);

            if (exists)
            {
                return -1;
            }

            articleProduct.Id = 0;
            var entity = this.mapper.Map<BlogArticleProductEntity>(articleProduct);

            await this.context.BlogArticleProducts.AddAsync(entity);
            await this.context.SaveChangesAsync();

            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteBlogArticleAsync(int articleId)
        {
            var existingArticle = await this.context.BlogArticles
                .Where(a => a.Id == articleId)
                .FirstOrDefaultAsync();

            if (existingArticle != null)
            {
                this.context.Remove(existingArticle);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteBlogArticleCommentAsync(int articleId, int commentId)
        {
            var existingComment = await this.context.BlogComments
                .Where(c => c.ArticleId == articleId && c.Id == commentId)
                .FirstOrDefaultAsync();

            if (existingComment != null)
            {
                this.context.Remove(existingComment);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteRelatedProductAsync(int articleId, int productId)
        {
            var existingProductArticle = await this.context.BlogArticleProducts
                .Where(p => p.ArticleId == articleId && p.ProductId == productId)
                .FirstOrDefaultAsync();

            if (existingProductArticle != null)
            {
                this.context.Remove(existingProductArticle);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<BlogArticle> GetBlogArticleAsync(int articleId)
        {
            var articleEntity = await this.context.BlogArticles
                .Where(a => a.Id == articleId)
                .FirstOrDefaultAsync();

            return this.mapper.Map<BlogArticle>(articleEntity);
        }

        /// <inheritdoc/>
        public async Task<BlogComment> GetBlogArticleCommentAsync(int articleId, int commentId)
        {
            var commentEntity = await this.context.BlogComments
                .Where(c => c.Id == commentId && c.ArticleId == articleId)
                .FirstOrDefaultAsync();

            return this.mapper.Map<BlogComment>(commentEntity);
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<BlogComment> GetBlogArticleCommentsAsync(int articleId, int offset, int limit)
        {
            var query = this.context.BlogComments
                .Where(c => c.ArticleId == articleId)
                .Skip(offset)
                .Take(limit);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<BlogComment>(entity);
            }
        }

        /// <inheritdoc/>
        public Task<int> CommentsCountAsync(int articleId)
        {
            return this.context.BlogComments.Where(c => c.ArticleId == articleId).CountAsync();
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync(int offset, int limit)
        {
            var query = this.context.BlogArticles
                .Skip(offset)
                .Take(limit);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<BlogArticle>(entity);
            }
        }

        /// <inheritdoc/>
        public Task<int> CountAsync()
        {
            return this.context.BlogArticles.CountAsync();
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<BlogArticleProduct> GetRelatedProductsAsync(int articleId)
        {
            var query = this.context.BlogArticleProducts.Where(p => p.ArticleId == articleId);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<BlogArticleProduct>(entity);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateBlogArticleAsync(int articleId, BlogArticle article)
        {
            article = article ?? throw new ArgumentNullException(nameof(article));

            var existingArticle = await this.context.BlogArticles
                .Where(a => a.Id == articleId)
                .FirstOrDefaultAsync();

            if (existingArticle != null)
            {
                existingArticle.Text = article.Text;
                existingArticle.Title = article.Title;
                existingArticle.Posted = article.Posted;

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateBlogArticleCommentAsync(int articleId, int commentId, BlogComment comment)
        {
            comment = comment ?? throw new ArgumentNullException(nameof(comment));

            var existingComment = await this.context.BlogComments
                .Where(c => c.ArticleId == articleId && c.Id == commentId)
                .FirstOrDefaultAsync();

            if (existingComment != null)
            {
                existingComment.Posted = comment.Posted;
                existingComment.Text = comment.Text;

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
