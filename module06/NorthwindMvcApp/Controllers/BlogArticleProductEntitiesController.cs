using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace NorthwindMvcApp.Controllers
{
    public class BlogArticleProductEntitiesController : Controller
    {
        private readonly BloggingContext _context;

        public BlogArticleProductEntitiesController(BloggingContext context)
        {
            _context = context;
        }

        // GET: BlogArticleProductEntities
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogArticleProducts.ToListAsync());
        }

        // GET: BlogArticleProductEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticleProductEntity = await _context.BlogArticleProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogArticleProductEntity == null)
            {
                return NotFound();
            }

            return View(blogArticleProductEntity);
        }

        // GET: BlogArticleProductEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogArticleProductEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArticleId,ProductId")] BlogArticleProductEntity blogArticleProductEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogArticleProductEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogArticleProductEntity);
        }

        // GET: BlogArticleProductEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticleProductEntity = await _context.BlogArticleProducts.FindAsync(id);
            if (blogArticleProductEntity == null)
            {
                return NotFound();
            }
            return View(blogArticleProductEntity);
        }

        // POST: BlogArticleProductEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArticleId,ProductId")] BlogArticleProductEntity blogArticleProductEntity)
        {
            if (id != blogArticleProductEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogArticleProductEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogArticleProductEntityExists(blogArticleProductEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogArticleProductEntity);
        }

        // GET: BlogArticleProductEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticleProductEntity = await _context.BlogArticleProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogArticleProductEntity == null)
            {
                return NotFound();
            }

            return View(blogArticleProductEntity);
        }

        // POST: BlogArticleProductEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogArticleProductEntity = await _context.BlogArticleProducts.FindAsync(id);
            _context.BlogArticleProducts.Remove(blogArticleProductEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogArticleProductEntityExists(int id)
        {
            return _context.BlogArticleProducts.Any(e => e.Id == id);
        }
    }
}
