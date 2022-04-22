using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Employees;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Employees controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeManagementService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="service">Service to retrieve employees.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public EmployeesController(IEmployeeManagementService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Gets employees.
        /// </summary>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>Employees collection.</returns>
        // GET: api/<EmployeesController>
        [HttpGet]
        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset = 0, int limit = 10)
        {
            await foreach (var employee in this.service.GetEmployeesAsync(offset, limit))
            {
                yield return employee;
            }
        }

        /// <summary>
        /// Gets employee by id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Employees if it exists; otherwise false.</returns>
        // GET api/<EmployeesController>/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeAsync(int id)
        {
            Employee employee = await this.service.GetEmployeeAsync(id);
            if (employee != null)
            {
                return this.Ok(employee);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Creates employee.
        /// </summary>
        /// <param name="employee">Employee to create.</param>
        /// <returns>Created employee.</returns>
        // POST api/<EmployeesController>
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployeeAsync(Employee employee)
        {
            if (employee is null)
            {
                return this.BadRequest();
            }

            int id = await this.service.CreateEmployeeAsync(employee);
            if (id > 0)
            {
                employee.Id = id;
                return this.CreatedAtAction(nameof(PostEmployeeAsync), employee);
            }
            else
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Updates employee.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="employee">Employee to update.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // PUT api/<EmployeesController>/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAsync(int id, Employee employee)
        {
            if (employee is null || id != employee.Id)
            {
                return this.BadRequest();
            }

            if (await this.service.UpdateEmployeeAsync(id, employee))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes employee.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // DELETE api/<EmployeesController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            if (await this.service.DestroyEmployeeAsync(id))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Gets employees count.
        /// </summary>
        /// <returns>Employees count.</returns>
        // GET api/<EmployeesController>/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetEmployeesCountAsync()
        {
            return this.Ok(await this.service.CountAsync());
        }
    }
}
