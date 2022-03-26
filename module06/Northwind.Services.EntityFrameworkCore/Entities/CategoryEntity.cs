using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class CategoryEntity
    {
        public CategoryEntity()
        {
            this.Products = new HashSet<ProductEntity>();
        }

        [Key]
        [Column("CategoryID")]
        public int Id { get; set; }
        [Required]
        [StringLength(15)]
        [Column("CategoryName")]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
