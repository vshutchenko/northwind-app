using System;
using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels
{
    /// <summary>
    /// Blog article view model.
    /// </summary>
    public class BlogArticleViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets publication date.
        /// </summary>
        [Required]
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        [Required]
        public int AuthorId { get; set; }
    }
}
