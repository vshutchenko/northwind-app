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
        public async Task<BlogArticle> GetBlogArticleAsync(int articleId)
        {
            var articleEntity = await this.context.BlogArticles
                .Where(a => a.Id == articleId)
                .FirstOrDefaultAsync();

            return this.mapper.Map<BlogArticle>(articleEntity);
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
    }
}
