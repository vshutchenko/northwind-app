using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Category;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        private readonly int pageSize = 10;

        public CategoryController(ApiClient apiClient, IMapper mapper)
        {
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            return View(new CategoryListViewModel
            {
                Categories = await this.apiClient
                  .GetCategoriesAsync((currentPage - 1) * pageSize, pageSize)
                  .Select(c => this.mapper.Map<CategoryViewModel>(c))
                  .ToListAsync(),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = await this.apiClient.GetCategoriesCountAsync(),
                }
            });
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CategoryInputViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var category = this.mapper.Map<ProductCategory>(inputModel);
                if (inputModel.NewPicture != null)
                {
                    category.Picture = await inputModel.NewPicture.GetBytesAsync();
                }

                var response = await this.apiClient.CreateCategoryAsync(category);

                if (!response.isCreated)
                {
                    ViewBag.Message = "Cannot create category";
                    return View("OperationCanceled");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.apiClient.GetCategoryAsync(id);

            var viewModel = this.mapper.Map<CategoryInputViewModel>(category);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryInputViewModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.BadRequest();
            }

            if (ModelState.IsValid)
            {
                var category = this.mapper.Map<ProductCategory>(inputModel);
                if (inputModel.NewPicture != null)
                {
                    category.Picture = await inputModel.NewPicture.GetBytesAsync();
                }

                var isUpdated = await this.apiClient.UpdateCategoryAsync(id, category);

                if (!isUpdated)
                {
                    ViewBag.Message = "Cannot update category";
                    return View("OperationCanceled");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await this.apiClient.GetCategoryAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            var viewModel = this.mapper.Map<CategoryViewModel>(category);

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await this.apiClient.DeleteCategoryAsync(id);

            if (!isDeleted)
            {
                ViewBag.Message = "Category was not deleted because some products reference to it";
                return View("OperationCanceled");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
