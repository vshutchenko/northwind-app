using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    /// <summary>
    /// Blog comment entity.
    /// </summary>
    [Table("comments")]
    public class BlogCommentEntity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        /// <summary>
        /// Gets or sets article id.
        /// </summary>
        [Required]
        [Column("article_id")]
        public int ArticleId { get; set; }
    }
}
