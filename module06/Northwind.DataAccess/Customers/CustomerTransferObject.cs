namespace Northwind.DataAccess.Customers
{
    /// <summary>
    /// Represents a TO for Northwind employees.
    /// </summary>
    public class CustomerTransferObject
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets company name.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets contact name.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets contact title.
        /// </summary>
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
