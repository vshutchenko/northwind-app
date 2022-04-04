using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using NorthwindMvcApp.ViewModels;

namespace NorthwindMvcApp.Controllers
{
    [Authorize]
    public class BlogArticlesController : Controller
    {
        private readonly IBloggingService blogService;
        private readonly IEmployeeManagementService employeeService;

        public BlogArticlesController(IBloggingService blogService, IEmployeeManagementService employeeService)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        // GET: BlogArticles
        public async Task<IActionResult> Index(int offset = 0, int limit = 100)
        {
            var articles = this.blogService.GetBlogArticlesAsync(offset, limit);
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

            return View(viewModels);
        }

        // GET: BlogArticles/Details/5
        public async Task<IActionResult> Details(int? id)
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
                comments.Add(new BlogCommentViewModel
                {
                    ArticleId = comment.ArticleId,
                    AuthorId = comment.AuthorId,
                    Id = comment.Id,
                    Posted = comment.Posted,
                    Text = comment.Text,
                });
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
                Comments = comments,
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

        // POST: BlogArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee,admin")]
        public async Task<IActionResult> Edit(int id, BlogArticleViewModel articleModel)
        {
            if (id != articleModel.Id)
            {
                return this.BadRequest();
            }

            var article = await this.blogService.GetBlogArticleAsync(id);
            if (article is null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                article.Text = articleModel.Text;
                article.Title = articleModel.Title;
                article.Posted = DateTime.Now;

                await this.blogService.UpdateBlogArticleAsync(id, article);

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
                if (await this.employeeService.GetEmployeeAsync(inputModel.AuthorId) is null)
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
