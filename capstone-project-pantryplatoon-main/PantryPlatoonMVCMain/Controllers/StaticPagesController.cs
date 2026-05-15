using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.ViewModels;

namespace PantryPlatoonMVCMain.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can use the static pages
    public class StaticPagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // For the main pages section
        string[] mainPageNames = new[] { "Home", "About", "Contact" };

        public StaticPagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Loads a list of all static pages
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            // Get all pages from database
            var pages = await _context.StaticPages.ToListAsync();

            // Split pages into main pages and subsections
            var mainPages = pages.Where(p => mainPageNames.Any(main =>
                                    string.Equals(main, p.PageName, StringComparison.OrdinalIgnoreCase)))
                                 .OrderBy(p => Array.IndexOf(mainPageNames.Select(n => n.ToLower()).ToArray(), p.PageName.ToLower()))
                                 .ToList();

            var subsectionPages = pages.Where(p => !mainPageNames.Any(main =>
                                         string.Equals(main, p.PageName, StringComparison.OrdinalIgnoreCase)))
                                      .OrderBy(p => p.PageName)
                                      .ToList();

            // Create a view model to pass both page lists
            var viewModel = new StaticPagesViewModel
            {
                MainPages = mainPages,
                SubsectionPages = subsectionPages
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staticPage = await _context.StaticPages
                .FirstOrDefaultAsync(m => m.PageId == id);

            if (staticPage == null)
            {
                return NotFound();
            }

            return View(staticPage);
        }

        // Create a new static page
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaticPages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("PageId,PageName,HtmlContent")] StaticPage staticPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staticPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staticPage);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staticPage = await _context.StaticPages.FindAsync(id);
            if (staticPage == null)
            {
                return NotFound();
            }
            return View(staticPage);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageId,PageName,HtmlContent")] StaticPage staticPage)
        {
            if (id != staticPage.PageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staticPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaticPageExists(staticPage.PageId))
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
            return View(staticPage);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staticPage = await _context.StaticPages
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (staticPage == null)
            {
                return NotFound();
            }

            return View(staticPage);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staticPage = await _context.StaticPages.FindAsync(id);
            if (staticPage != null)
            {
                _context.StaticPages.Remove(staticPage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool StaticPageExists(int id)
        {
            return _context.StaticPages.Any(e => e.PageId == id);
        }

    }
}