using System;
using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels
{
    /// <summary>
    /// Blog comment view model.
    /// </summary>
    public class BlogCommentViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets publication date.
        /// </summary>
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets article id.
        /// </summary>
        public int ArticleId { get; set; }
    }
}
