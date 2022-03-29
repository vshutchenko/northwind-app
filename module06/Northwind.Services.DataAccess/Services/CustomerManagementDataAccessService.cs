using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Northwind.DataAccess;
using Northwind.DataAccess.Customers;
using Northwind.Services.Customers;

namespace Northwind.Services.DataAccess.Services
{
    /// <inheritdoc/>
    public class CustomerManagementDataAccessService : ICustomerManagementService
    {
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">Factory to data access objects.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public CustomerManagementDataAccessService(NorthwindDataAccessFactory accessFactory, IMapper mapper)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Customer> GetCustomersAsync(int offset, int limit)
        {
            await foreach (var transfer in this.accessFactory
                .GetCustomerDataAccessObject()
                .SelectCustomersAsync(offset, limit))
            {
                yield return this.mapper.Map<Customer>(transfer);
            }
        }

        /// <inheritdoc/>
        public async Task<Customer> GetCustomerAsync(string customerId)
        {
            try
            {
                return this.mapper.Map<Customer>(await this.accessFactory
                .GetCustomerDataAccessObject()
                .FindCustomerAsync(customerId));
            }
            catch (CustomerNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public Task<string> CreateCustomerAsync(Customer customer)
        {
            customer = customer ?? throw new ArgumentNullException(nameof(customer));

            return this.accessFactory
                .GetCustomerDataAccessObject()
                .InsertCustomerAsync(this.mapper.Map<CustomerTransferObject>(customer));
        }

        /// <inheritdoc/>
        public Task<bool> DestroyCustomerAsync(string customerId)
        {
            return this.accessFactory.GetCustomerDataAccessObject().DeleteCustomerAsync(customerId);
        }

        /// <inheritdoc/>
        public Task<bool> UpdateCustomerAsync(string customerId, Customer customer)
        {
            customer = customer ?? throw new ArgumentNullException(nameof(customer));

            return this.accessFactory
                .GetCustomerDataAccessObject()
                .UpdateCustomerAsync(this.mapper.Map<CustomerTransferObject>(customer));
        }
    }
}
