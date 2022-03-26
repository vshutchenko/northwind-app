using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class ProductEntity
    {
        public ProductEntity()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("ProductID")]
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        [Column("ProductName")]
        public string Name { get; set; }
        [Column("SupplierID")]
        public int? SupplierId { get; set; }
        [Column("CategoryID")]
        public int? CategoryId { get; set; }
        [StringLength(20)]
        public string QuantityPerUnit { get; set; }
        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(CategoryEntity.Products))]
        public virtual CategoryEntity Category { get; set; }
        [ForeignKey(nameof(SupplierId))]
        [InverseProperty(nameof(Entities.Supplier.Products))]
        public virtual Supplier Supplier { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
