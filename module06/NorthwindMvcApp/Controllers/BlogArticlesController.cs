using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Blogging;
using NorthwindMvcApp.Models;
using NorthwindMvcApp.ViewModels;
using NorthwindMvcApp.ViewModels.Article;
using NorthwindMvcApp.ViewModels.Comment;
using NorthwindMvcApp.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<BlogArticleViewModel> viewModels = await this.blogService
                .GetBlogArticlesAsync(0, int.MaxValue)
                .Select(a => this.mapper.Map<BlogArticleViewModel>(a))
                .ToListAsync();

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

            List<BlogCommentViewModel> comments = await this.blogService.GetBlogArticleCommentsAsync(article.Id, 0, int.MaxValue)
                .Join(this.context.Users.AsAsyncEnumerable(), c => c.AuthorId, u => u.NorthwindDbId, (c, u) =>
                {
                    var commentViewModel = this.mapper.Map<BlogCommentViewModel>(c);
                    commentViewModel.AuthorName = u.Name;
                    return commentViewModel;
                })
                .ToListAsync();

            List<ProductViewModel> products = await this.blogService
                .GetRelatedProductsAsync(article.Id)
                .SelectAwait(async p => await this.apiClient.GetProductAsync(p.ProductId))
                .Select(product => this.mapper.Map<ProductViewModel>(product))
                .ToListAsync();

            var viewModel = new BlogArticleDetailsViewModel
            {
                AuthorId = article.AuthorId,
                Id = article.Id,
                Posted = article.Posted,
                Text = article.Text,
                Title = article.Title,
                AuthorName = $"{author.FirstName} {author.LastName}",
                AuthorPhoto = author.Photo,
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
        [Authorize(Roles = "employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> Create(BlogArticleViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var article = this.mapper.Map<BlogArticle>(inputModel);
                article.Posted = DateTime.Now;
                int id = await this.blogService.CreateBlogArticleAsync(article);
                
                if (id < 1)
                {
                    ViewBag.Message = "Cannot create article";
                    return View("OperationCanceled");
                }

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

            List<BlogArticleProduct> relatedProducts = await this.blogService.GetRelatedProductsAsync(id).ToListAsync();

            var viewModel = this.mapper.Map<BlogArticleInputViewModel>(article);

            viewModel.AllProducts = await this.apiClient.GetProductsAsync()
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                    Selected = relatedProducts.Any(rp => rp.ProductId == p.Id)
                })
                .ToListAsync();

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

                bool isUpdated = await this.blogService.UpdateBlogArticleAsync(id, article);

                if (!isUpdated)
                {
                    ViewBag.Message = "Cannot update product";
                    return View("OperationCanceled");
                }

                List<int> idsToDelete= await this.blogService
                   .GetRelatedProductsAsync(article.Id)
                   .Select(p => p.ProductId)
                   .ToListAsync();

                foreach (var idToDelete in idsToDelete)
                {
                    await this.blogService.DeleteRelatedProductAsync(article.Id, idToDelete);
                }

                if (inputModel.RelatedProductsIds != null)
                {
                    foreach (var prodId in inputModel.RelatedProductsIds)
                    {
                        await this.blogService.CreateRelatedProductAsync(new BlogArticleProduct(article.Id, prodId));
                    }
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
            List<int> relatedProductIdsToDelete = await this.blogService
                .GetRelatedProductsAsync(id)
                .Select(p => p.ProductId)
                .ToListAsync();

            foreach (var relatedProductId in relatedProductIdsToDelete)
            {
                await this.blogService.DeleteRelatedProductAsync(id, relatedProductId);
            }

            List<int> commentIdsToDelete = await this.blogService
                .GetBlogArticleCommentsAsync(id, 0, int.MaxValue)
                .Select(c => c.Id)
                .ToListAsync();

            foreach (var idToDelete in commentIdsToDelete)
            {
                await this.blogService.DeleteBlogArticleCommentAsync(id, idToDelete);
            }

            bool isDeleted = await this.blogService.DeleteBlogArticleAsync(id);
            if (!isDeleted)
            {
                ViewBag.Message = "Cannot delete";
                return View("OperationCanceled");
            }

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
