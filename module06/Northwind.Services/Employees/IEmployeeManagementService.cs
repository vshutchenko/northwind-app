using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Employees
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public interface IEmployeeManagementService
    {
        /// <summary>
        /// Gets a collection of employees using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="Employee"/>.</returns>
        IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit);

        /// <summary>
        /// Gets an employee with specified identifier.
        /// </summary>
        /// <param name="employeeId">An employee identifier.</param>
        /// <returns>Returns emloyee or null if employee does not exist.</returns>
        Task<Employee> GetEmployeeAsync(int employeeId);

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">An <see cref="Employee"/> to create.</param>
        /// <returns>An identifier of a created employee.</returns>
        Task<int> CreateEmployeeAsync(Employee employee);

        /// <summary>
        /// Destroys an existed employee.
        /// </summary>
        /// <param name="employeeId">An employee identifier.</param>
        /// <returns>True if an employee is destroyed; otherwise false.</returns>
        Task<bool> DestroyEmployeeAsync(int employeeId);

        /// <summary>
        /// Updates an employee.
        /// </summary>
        /// <param name="employeeId">An employee identifier.</param>
        /// <param name="employee">An <see cref="Employee"/>.</param>
        /// <returns>True if an employee is updated; otherwise false.</returns>
        Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee);

        /// <summary>
        /// Gets employees count.
        /// </summary>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        public Task<int> CountAsync();
    }
}
