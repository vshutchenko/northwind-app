using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Northwind.DataAccess;
using Northwind.DataAccess.Employees;
using Northwind.Services.Employees;

namespace Northwind.Services.DataAccess.Services
{
    /// <inheritdoc/>
    public class EmployeeManagementDataAccessService : IEmployeeManagementService
    {
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">Factory to data access objects.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public EmployeeManagementDataAccessService(NorthwindDataAccessFactory accessFactory, IMapper mapper)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
        {
            await foreach (var transfer in this.accessFactory
                .GetEmployeeDataAccessObject()
                .SelectEmployeesAsync(offset, limit))
            {
                yield return this.mapper.Map<Employee>(transfer);
            }
        }

        /// <inheritdoc/>
        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            try
            {
                return this.mapper.Map<Employee>(await this.accessFactory
                .GetEmployeeDataAccessObject()
                .FindEmployeeAsync(employeeId));
            }
            catch (EmployeeNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public Task<int> CreateEmployeeAsync(Employee employee)
        {
            employee = employee ?? throw new ArgumentNullException(nameof(employee));

            return this.accessFactory
                .GetEmployeeDataAccessObject()
                .InsertEmployeeAsync(this.mapper.Map<EmployeeTransferObject>(employee));
        }

        /// <inheritdoc/>
        public Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            return this.accessFactory.GetEmployeeDataAccessObject().DeleteEmployeeAsync(employeeId);
        }

        /// <inheritdoc/>
        public Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            employee = employee ?? throw new ArgumentNullException(nameof(employee));

            return this.accessFactory
                .GetEmployeeDataAccessObject()
                .UpdateEmployeeAsync(this.mapper.Map<EmployeeTransferObject>(employee));
        }

        /// <inheritdoc/>
        public Task<int> CountAsync()
        {
            return this.accessFactory
                .GetEmployeeDataAccessObject()
                .CountAsync();
        }
    }
}
