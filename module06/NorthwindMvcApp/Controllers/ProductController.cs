using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApiClient apiClient;
        private readonly IMapper mapper;
        private readonly int pageSize = 10;

        public ProductController(ApiClient apiClient, IMapper mapper)
        {
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: ProductController
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            List<ProductViewModel> productModels = new List<ProductViewModel>();
 
            await foreach (var p in this.apiClient.GetProductsAsync((currentPage - 1) * pageSize, pageSize))
            {
                var productModel = this.mapper.Map<ProductViewModel>(p);

                if (p.CategoryId != null)
                {
                    var category = await this.apiClient.GetCategoryAsync((int)p.CategoryId);    
                    productModel.CategoryName = category.Name;
                }
                else
                {
                    productModel.CategoryName = "Not specified";
                }

                productModels.Add(productModel);
            }

            return View(new ProductsListViewModel
            {
                Products = productModels,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = await this.apiClient.GetProductsCountAsync(),
                }
            });
        }

        // GET: ProductController/Create
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create()
        {
            List<SelectListItem> categoryItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "Not specified", Value = string.Empty, Selected = true }
            };

            await foreach (var c in this.apiClient.GetCategoriesAsync())
            {
                categoryItems.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            }

            return View(new ProductInputViewModel
            {
                CategoryItems = categoryItems,
            });
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(ProductInputViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var product = this.mapper.Map<Product>(inputModel);

                bool isCreated = await this.apiClient.CreateProductAsync(product);

                if (!isCreated)
                {
                    ViewBag.Message = "Cannot create product";
                    return View("OperationCanceled");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var product = await this.apiClient.GetProductAsync(id);

            var viewModel = this.mapper.Map<ProductInputViewModel>(product);

            List<SelectListItem> categoryItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "Not specified", Value = string.Empty, Selected = true }
            };

            await foreach (var c in this.apiClient.GetCategoriesAsync())
            {
                var item = new SelectListItem { Text = c.Name, Value = c.Id.ToString() };

                if (viewModel.CategoryId == c.Id)
                {
                    item.Selected = true;
                }

                categoryItems.Add(item);
            }

            viewModel.CategoryItems = categoryItems;
            return View(viewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int id, ProductInputViewModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.BadRequest();
            }

            if (ModelState.IsValid)
            {
                var product = this.mapper.Map<Product>(inputModel);
                
                bool isUpdated = await this.apiClient.UpdateProductAsync(id, product);

                if (!isUpdated)
                {
                    ViewBag.Message = "Cannot update product";
                    return View("OperationCanceled");
                }
            } 

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await this.apiClient.GetProductAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            var viewModel = this.mapper.Map<ProductViewModel>(product);

            return View(viewModel);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await this.apiClient.DeleteProductAsync(id);

            if (!isDeleted)
            {
                ViewBag.Message = "Product was not deleted because some articles or orders reference to it";
                return View("OperationCanceled");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
