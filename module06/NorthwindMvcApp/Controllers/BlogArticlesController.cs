using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Northwind.Services.Blogging;
using Northwind.Services.Customers;
using Northwind.Services.Employees;
using Northwind.Services.Products;
using NorthwindMvcApp.Models;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Article;
using NorthwindMvcApp.ViewModels.Comment;
using NorthwindMvcApp.ViewModels.Product;

namespace NorthwindMvcApp.Controllers
{
    [Authorize]
    public class BlogArticlesController : Controller
    {
        private readonly IBloggingService blogService;
        private readonly IEmployeeManagementService employeeService;
        private readonly ICustomerManagementService customerService;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;
        private readonly IdentityContext context;
        private readonly int pageSize = 10;

        public BlogArticlesController(IBloggingService blogService, IEmployeeManagementService employeeService, ICustomerManagementService customerService, IMapper mapper, IdentityContext context)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            this.customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
        }

        // GET: BlogArticles
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var articles = this.blogService.GetBlogArticlesAsync(0, int.MaxValue);
            List<BlogArticleViewModel> viewModels = new List<BlogArticleViewModel>();

            await foreach (var a in articles)
            {
                viewModels.Add(new BlogArticleViewModel
                {
                    AuthorId = a.AuthorId,
                    Id = a.Id,
                    Posted = a.Posted,
                    Text = a.Text,
                    Title = a.Title,
                });
            }

            var listModel = new BlogArticleListViewModel
            {
                Articles = viewModels.Skip((currentPage - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = viewModels.Count(),
                }
            };

            return View(listModel);
        }

        // GET: BlogArticles/Details/5
        public async Task<IActionResult> Details(int? id, int currentPage = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await this.blogService.GetBlogArticleAsync((int)id);
            if (article == null)
            {
                return NotFound();
            }

            var author = await this.employeeService.GetEmployeeAsync(article.AuthorId);
            List<BlogCommentViewModel> comments = new List<BlogCommentViewModel>();

            await foreach (var comment in this.blogService.GetBlogArticleCommentsAsync(article.Id, 0, 100))
            {
                var user = await this.context.Users.AsQueryable().Where(u => u.NorthwindDbId == comment.AuthorId).FirstOrDefaultAsync();
                if (user != null)
                {
                    comments.Add(new BlogCommentViewModel
                    {
                        ArticleId = comment.ArticleId,
                        AuthorId = comment.AuthorId,
                        Id = comment.Id,
                        Posted = comment.Posted,
                        Text = comment.Text,
                        AuthorName = user.Name,
                    });
                } 
            }

            List<ProductViewModel> relatedProducts = new List<ProductViewModel>();

            var json = await this.httpClient.GetStringAsync($"api/articles/{article.Id}/products");
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            foreach (var p in products)
            {
                relatedProducts.Add(this.mapper.Map<ProductViewModel>(p));
            }

            var viewModel = new BlogArticleDetailsViewModel
            {
                AuthorId = article.AuthorId,
                Id = article.Id,
                Posted = article.Posted,
                Text = article.Text,
                Title = article.Title,
                AuthorName = author is null ? "Unknown author" : $"{author.FirstName} {author.LastName}",
                AuthorPhoto = author is null ? Array.Empty<byte>() : author.Photo,
                CommentList = new CommentListViewModel
                {
                    Comments = comments.Skip((currentPage - 1) * 10).Take(10),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = currentPage,
                        ItemsPerPage = 10,
                        TotalItems = comments.Count,
                    }
                },
                RelatedProducts = relatedProducts,
            };

            return View(viewModel);
        }

        // GET: BlogArticles/Create
        [Authorize(Roles = "employee,admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Create(BlogArticleViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                if (await this.employeeService.GetEmployeeAsync(inputModel.AuthorId) is null)
                {
                    return this.NotFound();
                }

                var article = new BlogArticle
                {
                    AuthorId = inputModel.AuthorId,
                    Text = inputModel.Text,
                    Title = inputModel.Title,
                    Posted = DateTime.Now,
                };

                await this.blogService.CreateBlogArticleAsync(article);

                return RedirectToAction(nameof(Index));
            }

            return View(inputModel);
        }

        // GET: BlogArticles/Edit/5
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await this.blogService.GetBlogArticleAsync((int)id);
            if (article == null)
            {
                return NotFound();
            }

            List<ProductViewModel> relatedProducts = new List<ProductViewModel>();

            var jsonProducts = await this.httpClient.GetStringAsync($"api/products?offset={0}&limit={int.MaxValue}");
            var allProducts = JsonConvert.DeserializeObject<List<Product>>(jsonProducts);

            var json = await this.httpClient.GetStringAsync($"api/articles/{article.Id}/products");
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            foreach (var p in products)
            {
                relatedProducts.Add(this.mapper.Map<ProductViewModel>(p));
            }

            List<SelectListItem> relProducts = new List<SelectListItem>();

            foreach (var p in allProducts)
            {
                SelectListItem item = new SelectListItem { Text = p.Name, Value = p.Id.ToString() };

                if (relatedProducts.Any(rp => rp.Id == p.Id))
                {
                    item.Selected = true;
                }

                relProducts.Add(item);
            }


            var viewModel = new BlogArticleInputViewModel
            {
                AuthorId = article.AuthorId,
                Id = article.Id,
                Posted = article.Posted,
                Text = article.Text,
                Title = article.Title,
                AllProducts = relProducts,
            };

            return View(viewModel);
        }

        // POST: BlogArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Edit(int id, BlogArticleInputViewModel articleModel)
        {
            if (id != articleModel.Id)
            {
                return this.BadRequest();
            }

            if (ModelState.IsValid)
            {
                var article = new BlogArticle
                {
                    Id = id,
                    AuthorId = articleModel.AuthorId,
                    Posted = DateTime.Now,
                    Text = articleModel.Text,
                    Title = articleModel.Title,
                };

                await this.blogService.UpdateBlogArticleAsync(id, article);

                List<int> toDelete = new List<int>();
                await foreach (var relProd in this.blogService.GetRelatedProductsAsync(article.Id))
                {
                    toDelete.Add(relProd.ProductId);
                }

                foreach (var idtodel in toDelete)
                {
                    await this.blogService.DeleteRelatedProductAsync(article.Id, idtodel);
                }

                foreach (var prodId in articleModel.RelatedProductsIds)
                {
                    await this.blogService.CreateRelatedProductAsync(new BlogArticleProduct(article.Id, prodId));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(articleModel);
        }

        // GET: BlogArticles/Delete/5
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await this.blogService.GetBlogArticleAsync((int)id);

            if (article == null)
            {
                return NotFound();
            }

            var viewModel = new BlogArticleViewModel
            {
                AuthorId = article.AuthorId,
                Id = article.Id,
                Posted = article.Posted,
                Text = article.Text,
                Title = article.Title,
            };

            return View(viewModel);
        }

        // POST: BlogArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.blogService.DeleteBlogArticleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: BlogArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CreateComment(BlogCommentViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                if (await this.customerService.GetCustomerAsync(inputModel.AuthorId) is null)
                {
                    return this.NotFound();
                }

                var comment = new BlogComment
                {
                    AuthorId = inputModel.AuthorId,
                    Text = inputModel.Text,
                    Posted = DateTime.Now,
                    ArticleId = inputModel.ArticleId,
                    Id = inputModel.Id
                };

                await this.blogService.CreateBlogArticleCommentAsync(comment);
            }

            return RedirectToAction(nameof(Details), new { id = inputModel.ArticleId });
        }
    }
}
