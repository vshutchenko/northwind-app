using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Customers;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;

namespace Northwind.Services.EntityFrameworkCore.Services
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public class CustomerManagementService : ICustomerManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManagementService"/> class.
        /// </summary>
        /// <param name="context">Data base context.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public CustomerManagementService(NorthwindContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<string> CreateCustomerAsync(Customer customer)
        {
            customer = customer ?? throw new ArgumentNullException(nameof(customer));

            await this.context.Customers.AddAsync(this.mapper.Map<CustomerEntity>(customer));
            await this.context.SaveChangesAsync();

            return customer.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCustomerAsync(string customerId)
        {
            var existingCustomer = await this.context.Customers
                .Where(c => c.Id == customerId)
                .FirstOrDefaultAsync();

            if (existingCustomer != null)
            {
                this.context.Remove(existingCustomer);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<Customer> GetCustomerAsync(string customerId)
        {
            var customerEntity = await this.context.Customers
                .Where(c => c.Id == customerId)
                .FirstOrDefaultAsync();

            return this.mapper.Map<Customer>(customerEntity);
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Customer> GetCustomersAsync(int offset, int limit)
        {
            var query = this.context.Customers
                .Skip(offset)
                .Take(limit);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<Customer>(entity);
            }
        }

        /// <inheritdoc/>
        public Task<int> CountAsync()
        {
            return this.context.Customers.CountAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCustomerAsync(string customerId, Customer customer)
        {
            customer = customer ?? throw new ArgumentNullException(nameof(customer));

            var existingCustomer = await this.context.Customers
                .Where(c => c.Id == customerId)
                .FirstOrDefaultAsync();

            if (existingCustomer != null)
            {
                existingCustomer.CompanyName = customer.CompanyName;
                existingCustomer.City = customer.City;
                existingCustomer.Country = customer.Country;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Region = customer.Region;
                existingCustomer.Address = customer.Address;
                existingCustomer.ContactTitle = customer.ContactTitle;
                existingCustomer.ContactName = customer.ContactName;
                existingCustomer.PostalCode = customer.PostalCode;
                existingCustomer.Fax = customer.Fax;

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
