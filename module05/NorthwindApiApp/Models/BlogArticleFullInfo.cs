using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using System;

namespace NorthwindApiApp.Models
{
    /// <summary>
    /// Contains full information about article.
    /// </summary>
    public class BlogArticleFullInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleFullInfo"/> class.
        /// </summary>
        /// <param name="author">Author.</param>
        /// <param name="article">Article.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public BlogArticleFullInfo(Employee author, BlogArticle article)
        {
            author = author ?? throw new ArgumentNullException(nameof(author));
            article = article ?? throw new ArgumentNullException(nameof(article));

            this.Id = article.Id;
            this.Title = article.Title;
            this.Posted = article.Posted;
            this.Text = article.Text;

            this.AuthorId = author.Id;
            this.AuthorName = author.Title is null
                ? $"{author.FirstName} {author.LastName}"
                : $"{author.FirstName} {author.LastName}, {author.Title}";
        }

        /// <summary>
        /// Gets id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets publication date.
        /// </summary>
        public DateTime Posted { get; }

        /// <summary>
        /// Gets author id.
        /// </summary>
        public int AuthorId { get; }

        /// <summary>
        /// Gets author name.
        /// </summary>
        public string AuthorName { get; }

        /// <summary>
        /// Gets text.
        /// </summary>
        public string Text { get; }
    }
}
