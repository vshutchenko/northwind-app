using Microsoft.AspNetCore.Mvc.Rendering;
using NorthwindMvcApp.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NorthwindMvcApp.ViewModels.Article
{
    /// <summary>
    /// Blog article input view model.
    /// </summary>
    public class BlogArticleInputViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets publication date.
        /// </summary>
        [Required]
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        [Required]
        public int AuthorId { get; set; }

        public IEnumerable<int> RelatedProductsIds { get; set; }

        public IEnumerable<SelectListItem> AllProducts { get; set; }
    }
}
