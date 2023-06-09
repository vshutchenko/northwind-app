﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;

namespace Northwind.Services.EntityFrameworkCore.Services
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeManagementService"/> class.
        /// </summary>
        /// <param name="context">Data base context.</param>
        /// <param name="mapper">Mapper.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public EmployeeManagementService(NorthwindContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            employee = employee ?? throw new ArgumentNullException(nameof(employee));

            employee.Id = this.GenerateEmployeeId();
            await this.context.Employees.AddAsync(this.mapper.Map<EmployeeEntity>(employee));
            await this.context.SaveChangesAsync();

            return employee.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            var existingEmployee = await this.context.Employees
                .Where(e => e.Id == employeeId)
                .FirstOrDefaultAsync();

            if (existingEmployee != null)
            {
                this.context.Remove(existingEmployee);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
        {
            var query = this.context.Employees.Skip(offset).Take(limit);

            await foreach (var entity in query.AsAsyncEnumerable())
            {
                yield return this.mapper.Map<Employee>(entity);
            }
        }

        /// <inheritdoc/>
        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            var employee = await this.context.Employees
                .Where(e => e.Id == employeeId)
                .FirstOrDefaultAsync();

            return this.mapper.Map<Employee>(employee);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            employee = employee ?? throw new ArgumentNullException(nameof(employee));

            var existingEmployee = await this.context.Employees
                .Where(e => e.Id == employeeId)
                .FirstOrDefaultAsync();

            if (existingEmployee != null)
            {
                existingEmployee.Address = employee.Address;
                existingEmployee.City = employee.City;
                existingEmployee.Region = employee.Region;
                existingEmployee.BirthDate = employee.BirthDate;
                existingEmployee.Country = employee.Country;
                existingEmployee.Extension = employee.Extension;
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.HireDate = employee.HireDate;
                existingEmployee.PostalCode = employee.PostalCode;
                existingEmployee.HomePhone = employee.HomePhone;
                existingEmployee.TitleOfCourtesy = employee.TitleOfCourtesy;
                existingEmployee.Title = employee.Title;
                existingEmployee.Id = employee.Id;
                existingEmployee.PhotoPath = employee.PhotoPath;
                existingEmployee.ReportsTo = employee.ReportsTo;
                existingEmployee.Notes = employee.Notes;
                existingEmployee.Photo = employee.Photo;

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private int GenerateEmployeeId()
        {
            int id = this.context.Employees.Count() + 1;
            for (int i = 0; i < int.MaxValue; i++, id++)
            {
                if (!this.context.Employees.Any(p => p.Id == id))
                {
                    return id;
                }
            }

            return 0;
        }
    }
}
