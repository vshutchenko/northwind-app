using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.Customer
{
    /// <summary>
    /// Represents customer.
    /// </summary>
    public class CustomerViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Required]
        [RegularExpression(@"[A-Z]{5}")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets company name.
        /// </summary>
        [Required(ErrorMessage = "Company name is required.")]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets contact name.
        /// </summary>
        [Display(Name = "Contact name")]
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets contact title.
        /// </summary>
        [Display(Name = "Contact title")]
        public string ContactTitle { get; set; }

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
        /// Gets or sets region.
        /// </summary>
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets fax.
        /// </summary>
        public string Fax { get; set; }
    }
}
