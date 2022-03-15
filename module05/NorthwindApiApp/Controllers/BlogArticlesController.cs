using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.Products;
using NorthwindApiApp.ViewModels;
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
        private readonly IProductManagementService productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticlesController"/> class.
        /// </summary>
        /// <param name="blogService">Service to retrieve articles.</param>
        /// <param name="employeeService">Service to retrieve employees.</param>
        /// <param name="productService">Service to retrieve products.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public BlogArticlesController(IBloggingService blogService, IEmployeeManagementService employeeService, IProductManagementService productService)
        {
            this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// Gets articles.
        /// </summary>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>Articles collection.</returns>
        // GET: api/<BlogArticlesController>
        [HttpGet]
        public async IAsyncEnumerable<BlogArticleShortViewModel> GetBlogArticlesAsync(int offset = 0, int limit = 10)
        {
            await foreach (var article in this.blogService.GetBlogArticlesAsync(offset, limit))
            {
                var author = await this.employeeService.GetEmployeeAsync(article.AuthorId);
                if (!(author is null))
                {
                    yield return new BlogArticleShortViewModel(author, article);
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
        public async Task<ActionResult<BlogArticleFullViewModel>> GetBlogArticleAsync(int id)
        {
            BlogArticle article = await this.blogService.GetBlogArticleAsync(id);
                  
            if (article != null)
            {
                Employee author = await this.employeeService.GetEmployeeAsync(article.AuthorId);
                BlogArticleFullViewModel fullArticle = new BlogArticleFullViewModel(author, article);
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

        /// <summary>
        /// Gets all related products for article.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Products collection.</returns>
        // GET: api/<BlogArticlesController>/{article-id}/products
        [HttpGet("{article-id}/products")]
        public async IAsyncEnumerable<Product> GetBlogArticleProductsAsync([FromRoute(Name = "article-id")] int id)
        {
            var relatedProducts = this.blogService.GetRelatedProductsAsync(id);

            await foreach (var product in relatedProducts)
            {
                var p = await this.productService.GetProductAsync(product.ProductId);
                if (p != null)
                {
                    yield return p;
                }           
            }
        }

        /// <summary>
        /// Creates related product for article.
        /// </summary>
        /// <param name="articleId">Article id.</param>
        /// <param name="productId">Product id.</param>
        /// <returns></returns>
        // POST api/<BlogArticlesController>/{article-id}/products/{id}
        [HttpPost("{article-id}/products/{id}")]
        public async Task<ActionResult<BlogArticleProduct>> PostRelatedProductAsync(
            [FromRoute(Name = "article-id")] int articleId,
            [FromRoute(Name = "id")] int productId)
        {
            var article = await this.blogService.GetBlogArticleAsync(articleId);
            var product = await this.productService.GetProductAsync(productId);

            if (product is null || article is null)
            {
                return this.NotFound();
            }

            var blogArticleProduct = new BlogArticleProduct(articleId, productId);

            int id = await this.blogService.CreateRelatedProductAsync(blogArticleProduct);
            if (id > 0)
            {
                blogArticleProduct.Id = id;
                return this.CreatedAtAction(nameof(PostRelatedProductAsync), new { Id = id }, blogArticleProduct);
            }
            else
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Deletes related product for article.
        /// </summary>
        /// <param name="articleId">Article id.</param>
        /// <param name="productId">Product id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // DELETE api/<BlogArticlesController>/{article-id}/products/{id}
        [HttpDelete("{article-id}/products/{id}")]
        public async Task<IActionResult> DeleteRelatedProductAsync(
            [FromRoute(Name = "article-id")] int articleId,
            [FromRoute(Name = "id")] int productId)
        {
            bool isDeleted = await this.blogService.DeleteRelatedProductAsync(articleId, productId);

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
