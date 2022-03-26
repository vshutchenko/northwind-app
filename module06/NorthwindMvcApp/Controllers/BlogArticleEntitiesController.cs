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
    public class BlogArticleEntitiesController : Controller
    {
        private readonly BloggingContext _context;

        public BlogArticleEntitiesController(BloggingContext context)
        {
            _context = context;
        }

        // GET: BlogArticleEntities
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogArticles.ToListAsync());
        }

        // GET: BlogArticleEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticleEntity = await _context.BlogArticles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogArticleEntity == null)
            {
                return NotFound();
            }

            return View(blogArticleEntity);
        }

        // GET: BlogArticleEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogArticleEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,Posted,AuthorId")] BlogArticleEntity blogArticleEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogArticleEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogArticleEntity);
        }

        // GET: BlogArticleEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticleEntity = await _context.BlogArticles.FindAsync(id);
            if (blogArticleEntity == null)
            {
                return NotFound();
            }
            return View(blogArticleEntity);
        }

        // POST: BlogArticleEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,Posted,AuthorId")] BlogArticleEntity blogArticleEntity)
        {
            if (id != blogArticleEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogArticleEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogArticleEntityExists(blogArticleEntity.Id))
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
            return View(blogArticleEntity);
        }

        // GET: BlogArticleEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogArticleEntity = await _context.BlogArticles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogArticleEntity == null)
            {
                return NotFound();
            }

            return View(blogArticleEntity);
        }

        // POST: BlogArticleEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogArticleEntity = await _context.BlogArticles.FindAsync(id);
            _context.BlogArticles.Remove(blogArticleEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogArticleEntityExists(int id)
        {
            return _context.BlogArticles.Any(e => e.Id == id);
        }
    }
}
