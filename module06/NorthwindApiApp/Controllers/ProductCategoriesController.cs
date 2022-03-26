using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

#pragma warning disable CA2000 // Dispose objects before losing scope
namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Product categories controller.
    /// </summary>
    [Route("api/categories")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryManagementService categoryService;
        private readonly IProductCategoryPicturesService pictureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoriesController"/> class.
        /// </summary>
        /// <param name="categoryService">Service to retrieve categories.</param>
        /// <param name="pictureService">Service to retrieve pictures.</param>
        /// <exception cref="ArgumentNullException">Throws if one of parameters is null.</exception>
        public ProductCategoriesController(IProductCategoryManagementService categoryService, IProductCategoryPicturesService pictureService)
        {
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.pictureService = pictureService ?? throw new ArgumentNullException(nameof(pictureService));
        }

        /// <summary>
        /// Gets categories.
        /// </summary>
        /// <param name="offset">Offset.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>Categories collection.</returns>
        // GET: api/<ProductCategoriesController>
        [HttpGet]
        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset = 0, int limit = 10)
        {
            await foreach (var category in this.categoryService.GetCategoriesAsync(offset, limit))
            {
                yield return category;
            }
        }

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Category if it exists; otherwise false.</returns>
        // GET api/<ProductCategoriesController>/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetCategoryAsync(int id)
        {
            ProductCategory category = await this.categoryService.GetCategoryAsync(id);
            if (category != null)
            {
                return this.Ok(category);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Creates category.
        /// </summary>
        /// <param name="category">Category to create.</param>
        /// <returns>Created category.</returns>
        // POST api/<ProductCategoriesController>
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostCategoryAsync(ProductCategory category)
        {
            if (category is null)
            {
                return this.BadRequest();
            }

            int id = await this.categoryService.CreateCategoryAsync(category);
            if (id > 0)
            {
                return this.CreatedAtAction(nameof(PostCategoryAsync), new { Id = id }, category);
            }
            else
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Updates category.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="category">Category to update.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // PUT api/<ProductCategoriesController>/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryAsync(int id, ProductCategory category)
        {
            if (category is null || id != category.Id)
            {
                return this.BadRequest();
            }

            if (await this.categoryService.UpdateCategoriesAsync(id, category))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes category.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // DELETE api/<ProductCategoriesController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            bool isDeleted = await this.categoryService.DestroyCategoryAsync(id);

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
        /// Gets picture.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // GET api/<ProductCategoriesController>/{id}/picture
        [HttpGet("{id}/picture")]
        public async Task<ActionResult<byte[]>> GetPictureAsync(int id)
        {
            byte[] picture = await this.pictureService.GetPictureAsync(id);
            if (picture != null)
            {
                return this.File(picture, "image/jpeg");
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes picture.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // DELETE api/<ProductCategoriesController>/{id}/picture
        [HttpDelete("{id}/picture")]
        public async Task<IActionResult> DeletePictureAsync(int id)
        {
            if (await this.pictureService.DestroyPictureAsync(id))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Updates picture.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Returns <see cref="Task{IActionResult}"/>.</returns>
        // PUT api/<ProductCategoriesController>/{id}/picture
        [HttpPut("{id}/picture")]
        public async Task<IActionResult> PutPictureAsync(int id)
        {
            if (this.Request.Form.Files.Count == 0)
            {
                return this.BadRequest();
            }

            IFormFile file = this.Request.Form.Files[0];

            await using MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);

            if (await this.pictureService.UpdatePictureAsync(id, ms))
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
