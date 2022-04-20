using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.Employee
{
    public class EmployeeInputViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets title of courtesy.
        /// </summary>
        [Display(Name = "Title of courtesy")]
        public string TitleOfCourtesy { get; set; }

        /// <summary>
        /// Gets or sets birth date.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Birth date")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets hire date.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Hire date")]
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets postal code.
        /// </summary>
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets home phone.
        /// </summary>
        [Display(Name = "Home phone")]
        public string HomePhone { get; set; }

        /// <summary>
        /// Gets or sets extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets photo.
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// Gets or sets new  photo.
        /// </summary>
        [Display(Name = "New photo")]
        public IFormFile NewPhoto { get; set; }

        /// <summary>
        /// Gets or sets notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets id of employee report to.
        /// </summary>
        [Display(Name = "Reports to")]
        public int? ReportsTo { get; set; }

        /// <summary>
        /// Gets or sets photo path.
        /// </summary>
        [Display(Name = "Photo path")]
        public string PhotoPath { get; set; }

        /// <summary>
        /// Gets or sets employees list.
        /// </summary>
        [Display(Name = "Employees")]
        public IEnumerable<SelectListItem> EmployeeItems { get; set; }
    }
}
