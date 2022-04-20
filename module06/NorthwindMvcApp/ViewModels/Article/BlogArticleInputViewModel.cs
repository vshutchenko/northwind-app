using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "The title is required.")]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets publication date.
        /// </summary>
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        [Required(ErrorMessage = "The text is required.")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        public int AuthorId { get; set; }

        public IEnumerable<int> RelatedProductsIds { get; set; }
        
        [Display(Name = "Products")]
        public IEnumerable<SelectListItem> AllProducts { get; set; }
    }
}
