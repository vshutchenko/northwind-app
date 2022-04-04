using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Northwind.Services.Products;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        [Authorize(Roles = "admin,customer,employee")]
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

                var json = JsonConvert.SerializeObject(category, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.PostAsync("api/categories", content);

                if (response.IsSuccessStatusCode)
                {
                    var createdCategory = JsonConvert.DeserializeObject<ProductCategory>(await response.Content.ReadAsStringAsync());

                    if (inputModel.NewPicture != null)
                    {
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        var pictureBytes = await inputModel.NewPicture.GetBytesAsync();
                        form.Add(new ByteArrayContent(pictureBytes, 0, pictureBytes.Length), "categoryPicture", inputModel.Name);

                        response = await this.httpClient.PutAsync($"api/categories/{createdCategory.Id}/picture", form);
                    }
                }   
            }
            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var json = await this.httpClient.GetStringAsync($"/api/categories/{id}");
            var category = JsonConvert.DeserializeObject<ProductCategory>(json);

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

                var json = JsonConvert.SerializeObject(category, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.PutAsync($"api/categories/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var editedCategoryJson = await this.httpClient.GetStringAsync($"api/categories/{id}");
                    var editedCategory = JsonConvert.DeserializeObject<ProductCategory>(editedCategoryJson);

                    if (inputModel.NewPicture != null)
                    {
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        var pictureBytes = await inputModel.NewPicture.GetBytesAsync();
                        form.Add(new ByteArrayContent(pictureBytes, 0, pictureBytes.Length), "categoryPicture", inputModel.Name);

                        response = await this.httpClient.PutAsync($"api/categories/{editedCategory.Id}/picture", form);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await this.httpClient.GetAsync($"/api/categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var category = JsonConvert.DeserializeObject<ProductCategory>(await response.Content.ReadAsStringAsync());

            var viewModel = this.mapper.Map<CategoryViewModel>(category);

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await this.httpClient.DeleteAsync($"api/categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Category was succesfully deleted";
                return View("OperationCompleted");
            }
            else
            {
                ViewBag.Message = "Category was not deleted because some products reference to it";
                return View("OperationCanceled");
            }
        }
    }
}
