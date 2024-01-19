using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApplication_From_scratch_.Data;
using BlogApplication_From_scratch_.Models;
using System.Diagnostics;

namespace BlogApplication_From_scratch_.Controllers
{
    public class PostTagsController : Controller
    {
        private readonly BloggDbContext _context;

        public PostTagsController(BloggDbContext context)
        {
            _context = context;
        }

        // GET: PostTags
        public async Task<IActionResult> Index()
        {
            return _context.Tags != null ? 
                          View(await _context.Tags.ToListAsync()) :
                          Problem("Entity set 'BloggDbContext.Tags'  is null.");
        }

        // GET: PostTags/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var postTag = await _context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postTag == null)
            {
                return NotFound();
            }

            return View(postTag);
        }

        // GET: PostTags/Create
        public IActionResult Create()
        {
            try
            {
                // Retrieve tags from the database
                ViewBag.Tags = new SelectList(_context.Tags.ToList(), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                // Handle the exception, perhaps redirect to an error page
                return RedirectToAction("Error");
            }
        }



        // POST: PostTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DisplayName")] PostTag postTag)
        {
            if (ModelState.IsValid)
            {
                postTag.Id = Guid.NewGuid();
                _context.Add(postTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postTag);
        }

        // GET: PostTags/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var postTag = await _context.Tags.FindAsync(id);
            if (postTag == null)
            {
                return NotFound();
            }
            return View(postTag);
        }

        // POST: PostTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,DisplayName")] PostTag postTag)
        {
            if (id != postTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostTagExists(postTag.Id))
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
            return View(postTag);
        }

        // GET: PostTags/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var postTag = await _context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postTag == null)
            {
                return NotFound();
            }

            return View(postTag);
        }

        // POST: PostTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Tags == null)
            {
                return Problem("Entity set 'BloggDbContext.Tags'  is null.");
            }
            var postTag = await _context.Tags.FindAsync(id);
            if (postTag != null)
            {
                _context.Tags.Remove(postTag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostTagExists(Guid id)
        {
          return (_context.Tags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
