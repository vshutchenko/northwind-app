using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Services.Employees;
using NorthwindMvcApp.Models;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IdentityContext context;
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        private readonly int pageSize = 18;

        public EmployeeController(IdentityContext context, ApiClient apiClient, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            List<EmployeeViewModel> employeeModels = new List<EmployeeViewModel>();

            await foreach (var e in this.apiClient.GetEmployeesAsync())
            {
                employeeModels.Add(this.mapper.Map<EmployeeViewModel>(e));
            }

            return View(new EmployeeListViewModel
            {
                Employees = employeeModels.Skip((currentPage - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = employeeModels.Count(),
                }
            });
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> employeeItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "Not specified", Value = string.Empty, Selected = true }
            };

            await foreach (var e in this.apiClient.GetEmployeesAsync())
            {
                employeeItems.Add(new SelectListItem { Text = $"{e.FirstName} {e.LastName}", Value = e.Id.ToString() });
            }

            return View(new EmployeeInputViewModel
            {
                EmployeeItems = employeeItems,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(EmployeeInputViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var employee = this.mapper.Map<Employee>(inputModel);
                if (inputModel.NewPhoto != null)
                {
                    employee.Photo = await inputModel.NewPhoto.GetBytesAsync();
                }

                var response = await this.apiClient.CreateEmployeeAsync(employee);

                if (!response.isCreated)
                {
                    ModelState.AddModelError("", "Cannot create employee");
                    return View(inputModel);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await this.apiClient.GetEmployeeAsync(id);

            var viewModel = this.mapper.Map<EmployeeInputViewModel>(employee);

            List<SelectListItem> employeeItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "Not specified", Value = string.Empty, Selected = true }
            };

            await foreach (var e in this.apiClient.GetEmployeesAsync())
            {
                employeeItems.Add(new SelectListItem { Text = $"{e.FirstName} {e.LastName}", Value = e.Id.ToString() });
            }

            viewModel.EmployeeItems = employeeItems;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeInputViewModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.BadRequest();
            }

            if (ModelState.IsValid)
            {
                var employee = this.mapper.Map<Employee>(inputModel);
                if (inputModel.NewPhoto != null)
                {
                    employee.Photo = await inputModel.NewPhoto.GetBytesAsync();
                }

                var isUpdated = await this.apiClient.UpdateEmployeeAsync(id, employee);

                if (!isUpdated)
                {
                    ModelState.AddModelError("", "Cannot update employee");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await this.apiClient.GetEmployeeAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            var viewModel = this.mapper.Map<EmployeeViewModel>(employee);

            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await this.context.Users.AnyAsync(u => u.NorthwindDbId == id.ToString()))
            {
                ViewBag.Message = "Employee was not deleted because there is a connected account";
                return View("OperationCanceled");
            }

            bool isDeleted = await this.apiClient.DeleteEmployeeAsync(id);

            if (!isDeleted)
            {
                ViewBag.Message = "Employee was not deleted because some entities reference to it";
                return View("OperationCanceled");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
