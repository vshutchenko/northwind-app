using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Northwind.DataAccess.Customers
{
    /// <summary>
    /// The exception that is thrown when an employee is not found in a data storage.
    /// </summary>
    [Serializable]
#pragma warning disable CA1032 // Implement standard exception constructors
    public class CustomerNotFoundException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerNotFoundException"/> class with specified identifier and object type.
        /// </summary>
        /// <param name="id">A requested identifier.</param>
        public CustomerNotFoundException(string id)
            : base(string.Format(CultureInfo.InvariantCulture, $"A customer with identifier = {id}."))
        {
            this.CustomerId = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        protected CustomerNotFoundException(SerializationInfo info, StreamingContext context)
              : base(info, context)
        {
        }

        /// <summary>
        /// Gets an identifier of a customer that is missed in a data storage.
        /// </summary>
        public string CustomerId { get; }
    }
}
