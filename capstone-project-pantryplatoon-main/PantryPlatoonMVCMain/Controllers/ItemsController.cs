using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;
using PantryPlatoonMVCMain.Util;

namespace PantryPlatoonMVCMain.Controllers
{
    [Authorize(Roles ="Admin, Staff")]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Items.Include(i => i.ItemCategory);
            return View(await applicationDbContext.ToListAsync());
        }

         // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "ItemCategoryName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,Description,ItemCategoryId,Points,Quantity,Weight")] Item item, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Image upload and saving
                if (imageFile != null && imageFile.Length > 0)
                {
                    item.ImagePath = await SaveImage(imageFile);
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "ItemCategoryName", item.ItemCategoryId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "ItemCategoryName", item.ItemCategoryId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,Description,ItemCategoryId,Points,Quantity,Weight,ImagePath")] Item item, IFormFile? imageFile)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current item from database to compare ImagePath
                    var currentItem = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.ItemId == id);
                    if (currentItem == null)
                    {
                        return NotFound();
                    }

                    // Handle image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if it exists (use the current database value, not the form value)
                        if (!string.IsNullOrEmpty(currentItem.ImagePath))
                        {
                            DeleteImageFile(currentItem.ImagePath);
                        }

                        // Save new image
                        item.ImagePath = await SaveImage(imageFile);
                    }
                    else
                    {
                        // No new image uploaded, keep the existing one
                        item.ImagePath = currentItem.ImagePath;
                    }

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Admin");
            }
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "ItemCategoryName", item.ItemCategoryId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            // Get a new filename
            string newFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "items");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Save file
            string filePath = Path.Combine(uploadPath, newFileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return newFileName;
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            // Delete the image file
            if (!string.IsNullOrEmpty(item.ImagePath))
            {
                DeleteImageFile(item.ImagePath);
            }

            // Clear the ImagePath in the database
            item.ImagePath = null;
            await _context.SaveChangesAsync();

            // Redirect back to the edit page
            return RedirectToAction(nameof(Edit), new { id = item.ItemId });
        }

        private void DeleteImageFile(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "items", imagePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

    }
}