using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    /// <summary>
    /// Blog article entity.
    /// </summary>
    [Table("articles")]
    public class BlogArticleEntity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [Required]
        [StringLength(50)]
        [Column("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        [Required]
        [Column("text", TypeName = "ntext")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets publication date.
        /// </summary>
        [Required]
        [Column("posted", TypeName = "datetime")]
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        [Required]
        [Column("author_id")]
        public int AuthorId { get; set; }
    }
}
