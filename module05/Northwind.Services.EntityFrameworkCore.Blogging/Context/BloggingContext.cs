using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    /// <summary>
    /// Data base context.
    /// </summary>
    public class BloggingContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingContext"/> class.
        /// </summary>
        public BloggingContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingContext"/> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets blog articles.
        /// </summary>
        public DbSet<BlogArticleEntity> BlogArticles { get; set; }
    }
}
