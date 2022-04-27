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

        /// <summary>
        /// Creates customer.
        /// </summary>
        /// <param name="customer">Customer to create.</param>
        /// <returns>Created customer.</returns>
        // POST api/<CustomersController>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomerAsync(Customer customer)
        {
            if (customer is null)
            {
                return this.BadRequest();
            }

            bool isCreated = await this.service.CreateCustomerAsync(customer);
            if (isCreated)
            {
                return this.CreatedAtAction(nameof(PostCustomerAsync), customer);
            }
            else
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Updates customer.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="customer">Customer to update.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // PUT api/<CustomersController>/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAsync(string id, Customer customer)
        {
            if (customer is null || id != customer.Id)
            {
                return this.BadRequest();
            }

            if (await this.service.UpdateCustomerAsync(id, customer))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes customer.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // DELETE api/<CustomersController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(string id)
        {
            if (await this.service.DestroyCustomerAsync(id))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Gets customers count.
        /// </summary>
        /// <returns>Customers count.</returns>
        // GET api/<CustomersController>/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCustomersCountAsync()
        {
            return this.Ok(await this.service.CountAsync());
        }
    }
}
