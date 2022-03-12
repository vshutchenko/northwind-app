using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class Orders
    {
        public Orders()
        {
            this.OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("CustomerID")]
        [StringLength(5)]
        public string CustomerId { get; set; }
        [Column("EmployeeID")]
        public int? EmployeeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RequiredDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }
        [StringLength(40)]
        public string ShipName { get; set; }
        [StringLength(60)]
        public string ShipAddress { get; set; }
        [StringLength(15)]
        public string ShipCity { get; set; }
        [StringLength(15)]
        public string ShipRegion { get; set; }
        [StringLength(10)]
        public string ShipPostalCode { get; set; }
        [StringLength(15)]
        public string ShipCountry { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customers.Orders))]
        public virtual Customers Customer { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(EmployeesEntity.Orders))]
        public virtual EmployeesEntity Employee { get; set; }
        [ForeignKey(nameof(ShipVia))]
        [InverseProperty(nameof(Shippers.Orders))]
        public virtual Shippers ShipViaNavigation { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
