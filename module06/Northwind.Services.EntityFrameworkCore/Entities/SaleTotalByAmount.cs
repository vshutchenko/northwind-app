using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class SaleTotalByAmount
    {
        [Column(TypeName = "money")]
        public decimal? SaleAmount { get; set; }
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
    }
}
