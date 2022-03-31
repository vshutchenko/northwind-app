using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly int pageSize = 10;

        public CategoryController(IMapper mapper)
        {
            this.mapper = mapper;
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
        }

        // GET: CategoryController
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var json = await this.httpClient.GetStringAsync($"api/categories?offset={0}&limit={int.MaxValue}");

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            List<CategoryViewModel> viewModels = new List<CategoryViewModel>();

            foreach (var c in categories)
            {
                viewModels.Add(this.mapper.Map<CategoryViewModel>(c));
            }

            return View(new CategoryListViewModel
            {
                Categories = viewModels.Skip((currentPage - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = viewModels.Count(),
                }
            });
        }
    }
}
