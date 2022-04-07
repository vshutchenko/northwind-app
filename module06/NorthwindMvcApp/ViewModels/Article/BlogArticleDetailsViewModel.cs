using NorthwindMvcApp.ViewModels.Comment;
using NorthwindMvcApp.ViewModels.Product;
using System;
using System.Collections.Generic;

namespace NorthwindMvcApp.ViewModels
{
    public class BlogArticleDetailsViewModel
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

        /// <summary>
        /// Gets or sets author name.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets author photo.
        /// </summary>
        public byte[] AuthorPhoto { get; set; }

        public CommentListViewModel CommentList { get; set; }

        public IEnumerable<ProductViewModel> RelatedProducts { get; set; }
    }
}
