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
    public class BlogCommentEntitiesController : Controller
    {
        private readonly BloggingContext _context;

        public BlogCommentEntitiesController(BloggingContext context)
        {
            _context = context;
        }

        // GET: BlogCommentEntities
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogComments.ToListAsync());
        }

        // GET: BlogCommentEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCommentEntity = await _context.BlogComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogCommentEntity == null)
            {
                return NotFound();
            }

            return View(blogCommentEntity);
        }

        // GET: BlogCommentEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogCommentEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,Posted,AuthorId,ArticleId")] BlogCommentEntity blogCommentEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogCommentEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogCommentEntity);
        }

        // GET: BlogCommentEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCommentEntity = await _context.BlogComments.FindAsync(id);
            if (blogCommentEntity == null)
            {
                return NotFound();
            }
            return View(blogCommentEntity);
        }

        // POST: BlogCommentEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Posted,AuthorId,ArticleId")] BlogCommentEntity blogCommentEntity)
        {
            if (id != blogCommentEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogCommentEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogCommentEntityExists(blogCommentEntity.Id))
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
            return View(blogCommentEntity);
        }

        // GET: BlogCommentEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogCommentEntity = await _context.BlogComments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogCommentEntity == null)
            {
                return NotFound();
            }

            return View(blogCommentEntity);
        }

        // POST: BlogCommentEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogCommentEntity = await _context.BlogComments.FindAsync(id);
            _context.BlogComments.Remove(blogCommentEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogCommentEntityExists(int id)
        {
            return _context.BlogComments.Any(e => e.Id == id);
        }
    }
}
