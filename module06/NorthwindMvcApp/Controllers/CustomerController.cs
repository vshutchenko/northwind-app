using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Customers;
using NorthwindMvcApp.Models;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Customer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IdentityContext context;
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        private readonly int pageSize = 18;

        public CustomerController(IdentityContext context, ApiClient apiClient, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            return View(new CustomerListViewModel
            {
                Customers = await this.apiClient
                  .GetCustomersAsync((currentPage - 1) * pageSize, pageSize)
                  .Select(c => this.mapper.Map<CustomerViewModel>(c))
                  .ToListAsync(),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = await this.apiClient.GetCustomersCountAsync(),
                }
            });
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CustomerViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var customer = this.mapper.Map<Customer>(inputModel);

                var isCreated = await this.apiClient.CreateCustomerAsync(customer);

                if (!isCreated)
                {
                    ModelState.AddModelError("", "Cannot create customer");
                    return View(inputModel);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var customer = await this.apiClient.GetCustomerAsync(id);

            var viewModel = this.mapper.Map<CustomerViewModel>(customer);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CustomerViewModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.BadRequest();
            }

            if (ModelState.IsValid)
            {
                var customer = this.mapper.Map<Customer>(inputModel);

                var isUpdated = await this.apiClient.UpdateCustomerAsync(id, customer);

                if (!isUpdated)
                {
                    ModelState.AddModelError("", "Cannot update customer");
                    return View(inputModel);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await this.apiClient.GetCustomerAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            var viewModel = this.mapper.Map<CustomerViewModel>(customer);

            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await this.apiClient.GetCustomerAsync(id);
            var viewModel = this.mapper.Map<CustomerViewModel>(customer);

            if (await this.context.Users.AnyAsync(u => u.NorthwindDbId == id.ToString()))
            {            
                ModelState.AddModelError("", "Customer was not deleted because there is a connected account");
                return View(viewModel);
            }

            bool isDeleted = await this.apiClient.DeleteCustomerAsync(id);

            if (!isDeleted)
            {
                ModelState.AddModelError("", "Cannot delete customer");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
