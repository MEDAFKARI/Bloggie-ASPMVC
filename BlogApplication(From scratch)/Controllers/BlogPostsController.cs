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
    public class BlogPostsController : Controller
    {
        private readonly BloggDbContext _context;

        public BlogPostsController(BloggDbContext context)
        {
            _context = context;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index(string searchString, string tagSearchString)
        {
            var posts = _context.Posts.Include(p => p.Tags).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(p => p.Title.Contains(searchString) || p.Content.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(tagSearchString))
            {
                posts = posts.Where(p => p.Tags.Any(t => t.Name.Contains(tagSearchString)));
            }

            return View(await posts.ToListAsync());
        }



        public async Task<IActionResult> Gestion()
        {
            return _context.Posts != null ?
                        View(await _context.Posts.ToListAsync()) :
                        Problem("Entity set 'BloggDbContext.Posts'  is null.");
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Heading,Title,Content,ShortDescription,FeaturedImgUrl,UrlHandle,PublishedDate,Author,Visible")] BlogPost blogPost, string Tags)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tags = Tags.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tagString in tags)
                    {
                        var tag = new PostTag
                        {
                            Name = tagString,
                            DisplayName = tagString
                        };
                        _context.Tags.Add(tag); 
                        await _context.SaveChangesAsync();

                        blogPost.Tags.Add(tag);
                    }
                    blogPost.PublishedDate = DateTime.Now;
                    blogPost.Id = Guid.NewGuid();
                    _context.Add(blogPost);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(blogPost);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                return RedirectToAction("Error");
            }
        }


        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.Posts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Heading,Title,Content,ShortDescription,FeaturedImgUrl,UrlHandle,PublishedDate,Author,visible")] BlogPost blogPost, string Tags)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tags = Tags.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tagString in tags)
                    {
                        var tag = new PostTag
                        {
                            Name = tagString,
                            DisplayName = tagString
                        };
                        _context.Tags.Update(tag);
                        await _context.SaveChangesAsync();

                        blogPost.Tags.Add(tag);
                    }

                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
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
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'BloggDbContext.Posts'  is null.");
            }
            var blogPost = await _context.Posts.FindAsync(id);
            if (blogPost != null)
            {
                _context.Posts.Remove(blogPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(Guid id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
