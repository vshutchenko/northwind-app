﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets title of courtesy.
        /// </summary>
        public string TitleOfCourtesy { get; set; }

        /// <summary>
        /// Gets or sets birth date.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets hire date.
        /// </summary>
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
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets home phone.
        /// </summary>
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
        /// Gets or sets notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets id of employee report to.
        /// </summary>
        public int? ReportsTo { get; set; }

        /// <summary>
        /// Gets or sets photo path.
        /// </summary>
        public string PhotoPath { get; set; }
    }
}
