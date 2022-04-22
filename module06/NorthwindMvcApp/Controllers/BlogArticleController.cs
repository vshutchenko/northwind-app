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
    public class BlogArticleController : Controller
    {
        private readonly IBloggingService blogService;
        private readonly IMapper mapper;
        private readonly ApiClient apiClient;
        private readonly IdentityContext context;
        private readonly int pageSize = 10;
        private readonly IAuthorizationService authorizationService;

        public BlogArticleController(IBloggingService blogService, ApiClient apiClient, IdentityContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        // GET: BlogArticles
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var listModel = new BlogArticleListViewModel
            {
                Articles = await this.blogService
                    .GetBlogArticlesAsync((currentPage - 1) * pageSize, pageSize)
                    .Select(a => this.mapper.Map<BlogArticleViewModel>(a))
                    .ToListAsync(),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    TotalItems = await this.blogService.CountAsync(),
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

            List<BlogCommentViewModel> comments = await this.blogService.GetBlogArticleCommentsAsync(article.Id, (currentPage - 1) * 10, this.pageSize)
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
                    Comments = comments,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = currentPage,
                        ItemsPerPage = this.pageSize,
                        TotalItems = await this.blogService.CommentsCountAsync(article.Id),
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

                return RedirectToAction(nameof(Details), new { id = id });
            }

            return View(inputModel);
        }

        // GET: BlogArticles/Edit/5
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await this.blogService.GetBlogArticleAsync(id);

            if (article is null)
            {
                return this.NotFound();
            }

            var isAuthorized = await this.authorizationService.AuthorizeAsync(User, article, "ArticleAccessPolicy");

            if (!isAuthorized.Succeeded)
            {
                return this.Forbid();
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

                var isAuthorized = await this.authorizationService.AuthorizeAsync(User, article, "ArticleAccessPolicy");

                if (!isAuthorized.Succeeded)
                {
                    return this.Forbid();
                }

                bool isUpdated = await this.blogService.UpdateBlogArticleAsync(id, article);

                if (!isUpdated)
                {
                    ViewBag.Message = "Cannot update product";
                    return View("OperationCanceled");
                }

                await this.DeleteRelatedProducts(article.Id);

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

            if (article is null)
            {
                return this.NotFound();
            }

            var isAuthorized = await this.authorizationService.AuthorizeAsync(User, article, "ArticleAccessPolicy");

            if (!isAuthorized.Succeeded)
            {
                return this.Forbid();
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
            var article = await this.blogService.GetBlogArticleAsync(id);

            if (article is null)
            {
                return this.NotFound();
            }

            var isAuthorized = await this.authorizationService.AuthorizeAsync(User, article, "ArticleAccessPolicy");

            if (!isAuthorized.Succeeded)
            {
                return this.Forbid();
            }

            await this.DeleteRelatedProducts(id);
            await this.DeleteComments(id);

            bool isDeleted = await this.blogService.DeleteBlogArticleAsync(id);
            if (!isDeleted)
            {
                ViewBag.Message = "Cannot delete article";
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

        [Authorize(Roles = "admin,customer")]
        public async Task<IActionResult> EditComment(int articleId, int commentId)
        {
            var comment = await this.blogService.GetBlogArticleCommentAsync(articleId, commentId);

            if (comment is null)
            {
                return this.NotFound();
            }

            var isAuthorized = await this.authorizationService.AuthorizeAsync(User, comment, "CommentAccessPolicy");

            if (!isAuthorized.Succeeded)
            {
                return this.Forbid();
            }

            var commentModel = this.mapper.Map<BlogCommentViewModel>(comment);

            return View(commentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,customer")]
        public async Task<IActionResult> EditComment(int id, BlogCommentViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var comment = this.mapper.Map<BlogComment>(inputModel);
                comment.Posted = DateTime.Now;

                var isAuthorized = await this.authorizationService.AuthorizeAsync(User, comment, "CommentAccessPolicy");

                if (!isAuthorized.Succeeded)
                {
                    return this.Forbid();
                }

                bool isUpdated = await this.blogService.UpdateBlogArticleCommentAsync(inputModel.ArticleId, inputModel.Id, comment);
                if (!isUpdated)
                {
                    return this.NotFound();
                }
            }

            return RedirectToAction(nameof(Details), new { id = inputModel.ArticleId });
        }

        [Authorize(Roles = "customer,admin")]
        public async Task<IActionResult> DeleteComment(int articleId, int commentId)
        {
            var comment = await this.blogService.GetBlogArticleCommentAsync(articleId, commentId);

            if (comment is null)
            {
                return this.NotFound();
            }

            var isAuthorized = await this.authorizationService.AuthorizeAsync(User, comment, "CommentAccessPolicy");

            if (!isAuthorized.Succeeded)
            {
                return this.Forbid();
            }

            var viewModel = this.mapper.Map<BlogCommentViewModel>(comment);

            return View(viewModel);
        }

        // POST: BlogArticles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "customer,admin")]
        public async Task<IActionResult> DeleteCommentConfirmed(int articleId, int commentId)
        {
            var comment = await this.blogService.GetBlogArticleCommentAsync(articleId, commentId);

            if (comment is null)
            {
                return this.NotFound();
            }

            var isAuthorized = await this.authorizationService.AuthorizeAsync(User, comment, "CommentAccessPolicy");

            if (!isAuthorized.Succeeded)
            {
                return this.Forbid();
            }

            await this.blogService.DeleteBlogArticleCommentAsync(articleId, commentId);

            return RedirectToAction(nameof(Details), new { id = articleId });
        }

        private async Task DeleteRelatedProducts(int articleId)
        {
            List<int> relatedProductIdsToDelete = await this.blogService
                .GetRelatedProductsAsync(articleId)
                .Select(p => p.ProductId)
                .ToListAsync();

            foreach (var relatedProductId in relatedProductIdsToDelete)
            {
                await this.blogService.DeleteRelatedProductAsync(articleId, relatedProductId);
            }
        }

        private async Task DeleteComments(int articleId)
        {
            List<int> commentIdsToDelete = await this.blogService
                .GetBlogArticleCommentsAsync(articleId, 0, int.MaxValue)
                .Select(c => c.Id)
                .ToListAsync();

            foreach (var idToDelete in commentIdsToDelete)
            {
                await this.blogService.DeleteBlogArticleCommentAsync(articleId, idToDelete);
            }
        }
    }
}
