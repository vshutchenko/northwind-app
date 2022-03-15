using System;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents blog comment.
    /// </summary>
    public class BlogComment
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Gets or sets article id.
        /// </summary>
        public int ArticleId { get; set; }
    }
}
