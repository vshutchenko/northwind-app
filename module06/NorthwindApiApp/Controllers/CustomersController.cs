using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Customers;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Customers controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerManagementService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="service">Service to retrieve customers.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public CustomersController(ICustomerManagementService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Gets customers.
        /// </summary>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>Customers collection.</returns>
        // GET: api/<CustomersController>
        [HttpGet]
        public async IAsyncEnumerable<Customer> GetCustomersAsync(int offset = 0, int limit = 10)
        {
            await foreach (var customer in this.service.GetCustomersAsync(offset, limit))
            {
                yield return customer;
            }
        }

        /// <summary>
        /// Gets customer by id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Customer if it exists; otherwise false.</returns>
        // GET api/<CustomersController>/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerAsync(string id)
        {
            Customer customer = await this.service.GetCustomerAsync(id);
            if (customer != null)
            {
                return this.Ok(customer);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
