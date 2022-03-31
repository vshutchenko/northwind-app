using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
