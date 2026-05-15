using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone2025_PantryPlatoon.Data;
using Capstone2025_PantryPlatoon.Models;

namespace Capstone2025_PantryPlatoon.Controllers
{
    public class StaticPagesController : Controller
    {
        private readonly PantryPlatoonDbContext _context;

        public StaticPagesController(PantryPlatoonDbContext context)
        {
            _context = context;
        }

        // GET: StaticPages
        public async Task<IActionResult> Index()
        {
            return View(await _context.StaticPages.ToListAsync());
        }

        // GET: StaticPages/Details/5
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

        // GET: StaticPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaticPages/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: StaticPages/Edit/5
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

        // POST: StaticPages/Edit/5        
        [HttpPost]
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

        // GET: StaticPages/Delete/5
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

        // POST: StaticPages/Delete/5
        [HttpPost, ActionName("Delete")]
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

        private bool StaticPageExists(int id)
        {
            return _context.StaticPages.Any(e => e.PageId == id);
        }
    }
}
