using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthwindMvcApp.Models;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IdentityContext context;
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        private readonly int pageSize = 10;

        public UserController(IdentityContext context, ApiClient apiClient, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var userModels = this.context.Users
                .Include(u => u.Role)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(u => this.mapper.Map<UserViewModel>(u))
                .ToList();

            return View(new UserListViewModel
            {
                Users = userModels,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = this.context.Users.Count(),
                }
            });
        }

        public async Task<IActionResult> CreateEmployeeUser()
        {
            List<SelectListItem> employeeItems = await this.apiClient
                .GetEmployeesAsync()
                .Where(e => !this.context.Users.Any(u => u.NorthwindDbId == e.Id.ToString()))
                .Select(e => new SelectListItem { Text = $"{e.FirstName} {e.LastName}", Value = e.Id.ToString() })
                .ToListAsync();

            return View("Create", new UserInputViewModel
            {
                Entities = employeeItems,
                RoleId = 2,
            });
        }

        public async Task<IActionResult> CreateCustomerUser()
        {
            List<SelectListItem> customerItems = await this.apiClient
                .GetCustomersAsync()
                .Where(c => !this.context.Users.Any(u => u.NorthwindDbId == c.Id))
                .Select(c => new SelectListItem { Text = $"{c.CompanyName}", Value = c.Id.ToString() })
                .ToListAsync();

            return View("Create", new UserInputViewModel
            {
                Entities = customerItems,
                RoleId = 3,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserInputViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                await this.context.Users.AddAsync(new User
                {
                    Name = inputModel.Name,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(inputModel.Password),
                    RoleId = inputModel.RoleId,
                    NorthwindDbId = inputModel.NorthwindDbId,
                });

                await this.context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = this.context.Users.Include(u => u.Role).Where(u => u.Id == id).FirstOrDefault();

            if (user is null)
            {
                return NotFound();
            }

            var viewModel = this.mapper.Map<UserViewModel>(user);

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = this.context.Users.AsQueryable().Where(u => u.Id == id).FirstOrDefault();
            if (user is null)
            {
                ViewBag.Message = "User does not exist";
                return View("OperationCanceled");
            }

            this.context.Users.Remove(user);

            await this.context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
