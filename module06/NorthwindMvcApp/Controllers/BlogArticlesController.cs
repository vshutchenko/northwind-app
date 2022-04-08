using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Northwind.Services.Blogging;
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
        private readonly IMapper mapper;
        private readonly ApiClient apiClient;
        private readonly IdentityContext context;
        private readonly int pageSize = 10;

        public BlogArticlesController(IBloggingService blogService, ApiClient apiClient, IdentityContext context, IMapper mapper)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: BlogArticles
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var articles = this.blogService.GetBlogArticlesAsync(0, int.MaxValue);
            List<BlogArticleViewModel> viewModels = new List<BlogArticleViewModel>();

            await foreach (var a in articles)
            {
                viewModels.Add(this.mapper.Map<BlogArticleViewModel>(a));
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
        public async Task<IActionResult> Details(int id, int currentPage = 1)
        {
            var article = await this.blogService.GetBlogArticleAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var author = await this.apiClient.GetEmployeeAsync(article.AuthorId);
            List<BlogCommentViewModel> comments = new List<BlogCommentViewModel>();

            await foreach (var comment in this.blogService.GetBlogArticleCommentsAsync(article.Id, 0, int.MaxValue))
            {
                var user = await this.context.Users.AsQueryable().Where(u => u.NorthwindDbId == comment.AuthorId).FirstOrDefaultAsync();
                if (user != null)
                {
                    var commentViewModel = this.mapper.Map<BlogCommentViewModel>(comment);
                    commentViewModel.AuthorName = user.Name;
                    comments.Add(commentViewModel);
                } 
            }

            List<ProductViewModel> products = new List<ProductViewModel>();

            await foreach (var relatedProduct in this.blogService.GetRelatedProductsAsync(article.Id))
            {
                var product = await this.apiClient.GetProductAsync(relatedProduct.ProductId);
                products.Add(this.mapper.Map<ProductViewModel>(product));
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
                RelatedProducts = products,
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
                var article = this.mapper.Map<BlogArticle>(inputModel);
                article.Posted = DateTime.Now;
                await this.blogService.CreateBlogArticleAsync(article);

                return RedirectToAction(nameof(Index));
            }

            return View(inputModel);
        }

        // GET: BlogArticles/Edit/5
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await this.blogService.GetBlogArticleAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            List<BlogArticleProduct> relatedProducts = new List<BlogArticleProduct>();

            await foreach(var p in this.blogService.GetRelatedProductsAsync(id))
            {
                relatedProducts.Add(p);
            }

            List<SelectListItem> selectItems = new List<SelectListItem>();

            await foreach (var p in this.apiClient.GetProductsAsync())
            {
                SelectListItem item = new SelectListItem { Text = p.Name, Value = p.Id.ToString() };

                if (relatedProducts.Any(rp => rp.Id == p.Id))
                {
                    item.Selected = true;
                }

                selectItems.Add(item);
            }


            var viewModel = this.mapper.Map<BlogArticleInputViewModel>(article);

            viewModel.AllProducts = selectItems;

            return View(viewModel);
        }

        // POST: BlogArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Edit(int id, BlogArticleInputViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var article = this.mapper.Map<BlogArticle>(inputModel);
                article.Posted = DateTime.Now;

                await this.blogService.UpdateBlogArticleAsync(id, article);

                List<int> idsToDelete = new List<int>();
                await foreach (var relProd in this.blogService.GetRelatedProductsAsync(article.Id))
                {
                    idsToDelete.Add(relProd.ProductId);
                }

                foreach (var idToDelete in idsToDelete)
                {
                    await this.blogService.DeleteRelatedProductAsync(article.Id, idToDelete);
                }

                foreach (var prodId in inputModel.RelatedProductsIds)
                {
                    await this.blogService.CreateRelatedProductAsync(new BlogArticleProduct(article.Id, prodId));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(inputModel);
        }

        // GET: BlogArticles/Delete/5
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await this.blogService.GetBlogArticleAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            var viewModel = this.mapper.Map<BlogArticleViewModel>(article);

            return View(viewModel);
        }

        // POST: BlogArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<int> relatedProductIdsToDelete = new List<int>();
            await foreach (var relatedProduct in this.blogService.GetRelatedProductsAsync(id))
            {
                relatedProductIdsToDelete.Add(relatedProduct.ProductId);
            }

            foreach (var relatedProductId in relatedProductIdsToDelete)
            {
                await this.blogService.DeleteRelatedProductAsync(id, relatedProductId);
            }

            List<int> commentIdsToDelete = new List<int>();

            await foreach (var idToDelete in this.blogService.GetBlogArticleCommentsAsync(id, 0, int.MaxValue).Select(c => c.Id))
            {
                commentIdsToDelete.Add(idToDelete);
            }

            foreach (var idToDelete in commentIdsToDelete)
            {
                await this.blogService.DeleteBlogArticleCommentAsync(id, idToDelete);
            }

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
                if (await this.apiClient.GetCustomerAsync(inputModel.AuthorId) is null)
                {
                    return this.NotFound();
                }

                var comment = this.mapper.Map<BlogComment>(inputModel);

                comment.Posted = DateTime.Now;

                await this.blogService.CreateBlogArticleCommentAsync(comment);
            }

            return RedirectToAction(nameof(Details), new { id = inputModel.ArticleId });
        }
    }
}
