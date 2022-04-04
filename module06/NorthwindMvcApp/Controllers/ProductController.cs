using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Category;
using NorthwindMvcApp.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly int pageSize = 10;

        public ProductController(IMapper mapper)
        {
            this.mapper = mapper;
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
        }

        // GET: ProductController
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var json = await this.httpClient.GetStringAsync($"api/products?offset={0}&limit={int.MaxValue}");

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            List<ProductViewModel> viewModels = new List<ProductViewModel>();

            foreach (var p in products)
            {
                var viewModel = this.mapper.Map<ProductViewModel>(p);

                if (p.CategoryId != null)
                {
                    var categoryJson = await this.httpClient.GetStringAsync($"api/categories/{p.CategoryId}");
                    var category = JsonConvert.DeserializeObject<ProductCategory>(categoryJson);                 
                    viewModel.CategoryName = category.Name;
                }
                else
                {
                    viewModel.CategoryName = "Not specified";
                }

                viewModels.Add(viewModel);
            }

            return View(new ProductsListViewModel
            {
                Products = viewModels.Skip((currentPage - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = viewModels.Count(),
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

            await foreach (var c in this.GetCategoryViewModelsAsync())
            {
                categoryItems.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            }

            return View(new ProductInputViewModel
            {
                Categories = categoryItems,
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

                var json = JsonConvert.SerializeObject(product, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.PostAsync("api/products", content);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var json = await this.httpClient.GetStringAsync($"/api/products/{id}");
            var product = JsonConvert.DeserializeObject<Product>(json);

            var viewModel = this.mapper.Map<ProductInputViewModel>(product);

            List<SelectListItem> categoryItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "Not specified", Value = string.Empty, Selected = true }
            };

            await foreach (var c in this.GetCategoryViewModelsAsync())
            {
                var item = new SelectListItem { Text = c.Name, Value = c.Id.ToString() };
                if (viewModel.CategoryId == c.Id)
                {
                    item.Selected = true;
                }

                categoryItems.Add(item);
            }

            viewModel.Categories = categoryItems;
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

                var json = JsonConvert.SerializeObject(product, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.PutAsync($"api/products/{id}", content);
            } 

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await this.httpClient.GetAsync($"/api/products/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());

            var viewModel = this.mapper.Map<ProductViewModel>(product);

            return View(viewModel);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var response = await this.httpClient.DeleteAsync($"api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Product was succesfully deleted";
                return View("OperationCompleted");
            }
            else
            {
                ViewBag.Message = "Product was not deleted because some articles or orders reference to it";
                return View("OperationCanceled");
            }
        }

        private async IAsyncEnumerable<CategoryViewModel> GetCategoryViewModelsAsync()
        {
            var json = await this.httpClient.GetStringAsync($"api/categories?offset={0}&limit={int.MaxValue}");

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            foreach (var c in categories)
            {
                yield return this.mapper.Map<CategoryViewModel>(c);
            }
        }
    }
}
