using System;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Blog article.
    /// </summary>
    public class BlogArticle
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets publication date.
        /// </summary>
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        public int AuthorId { get; set; }
    }
}
