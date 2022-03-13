using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using NorthwindApiApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Blog articles controller.
    /// </summary>
    [Route("api/articles")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService blogService;
        private readonly IEmployeeManagementService employeeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticlesController"/> class.
        /// </summary>
        /// <param name="blogService">Service to retrieve articles.</param>
        /// <param name="employeeService">Service to retrieve employees.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public BlogArticlesController(IBloggingService blogService, IEmployeeManagementService employeeService)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        /// <summary>
        /// Gets articles.
        /// </summary>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>Articles collection.</returns>
        // GET: api/<BlogArticlesController>
        [HttpGet]
        public async IAsyncEnumerable<BlogArticleShortInfo> GetBlogArticlesAsync(int offset = 0, int limit = 10)
        {
            await foreach (var article in this.blogService.GetBlogArticlesAsync(offset, limit))
            {
                var author = await this.employeeService.GetEmployeeAsync(article.AuthorId);
                if (!(author is null))
                {
                    yield return new BlogArticleShortInfo(author, article);
                }
                
            }
        }

        /// <summary>
        /// Gets article by id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Article if it exists; otherwise false.</returns>
        // GET api/<BlogArticlesController>/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogArticleFullInfo>> GetBlogArticleAsync(int id)
        {
            BlogArticle article = await this.blogService.GetBlogArticleAsync(id);
                  
            if (article != null)
            {
                Employee author = await this.employeeService.GetEmployeeAsync(article.AuthorId);
                BlogArticleFullInfo fullArticle = new BlogArticleFullInfo(author, article);
                return this.Ok(fullArticle);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Creates article.
        /// </summary>
        /// <param name="article">Article to create.</param>
        /// <returns>Created category.</returns>
        // POST api/<BlogArticlesController>
        [HttpPost]
        public async Task<ActionResult<BlogArticle>> PostBlogArticleAsync(BlogArticle article)
        {
            if (article is null)
            {
                return this.BadRequest();
            }

            var author = await this.employeeService.GetEmployeeAsync(article.AuthorId);

            if (author is null)
            {
                return this.NotFound();
            }

            article.Posted = DateTime.Now;

            int id = await this.blogService.CreateBlogArticleAsync(article);
            if (id > 0)
            {
                article.Id = id;
                return this.CreatedAtAction(nameof(PostBlogArticleAsync), new { Id = id }, article);
            }
            else
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Updates article.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="article">Article to update.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // PUT api/<BlogArticlesController>/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogArticleAsync(int id, BlogArticle article)
        {
            if (article is null)
            {
                return this.BadRequest();
            }

            article.Posted = DateTime.Now;

            if (await this.blogService.UpdateBlogArticleAsync(id, article))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes article.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // DELETE api/<BlogArticlesController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogArticleAsync(int id)
        {
            bool isDeleted = await this.blogService.DeleteBlogArticleAsync(id);

            if (isDeleted)
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
