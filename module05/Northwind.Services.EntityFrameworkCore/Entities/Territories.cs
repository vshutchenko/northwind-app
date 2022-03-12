using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class Territories
    {
        public Territories()
        {
            this.EmployeeTerritories = new HashSet<EmployeeTerritories>();
        }

        [Key]
        [Column("TerritoryID")]
        [StringLength(20)]
        public string TerritoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string TerritoryDescription { get; set; }
        [Column("RegionID")]
        public int RegionId { get; set; }

        [ForeignKey(nameof(RegionId))]
        [InverseProperty("Territories")]
        public virtual Region Region { get; set; }
        [InverseProperty("Territory")]
        public virtual ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}
