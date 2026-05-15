using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.Pages.Documentation
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public List<DocumentationUpload> Documents { get; set; } = new List<DocumentationUpload>();

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDocumentsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                TempData["ErrorMessage"] = "Please enter a document title.";
                await LoadDocumentsAsync();
                return Page();
            }

            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please choose a file to upload.";
                await LoadDocumentsAsync();
                return Page();
            }

            string[] allowedExtensions =
            {
                ".pdf",
                ".doc",
                ".docx",
                ".txt",
                ".png",
                ".jpg",
                ".jpeg"
            };

            string extension = Path.GetExtension(UploadedFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                TempData["ErrorMessage"] = "Only PDF, Word, TXT, PNG, JPG, and JPEG files are allowed.";
                await LoadDocumentsAsync();
                return Page();
            }

            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "documentation"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string storedFileName = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(uploadsFolder, storedFileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await UploadedFile.CopyToAsync(stream);
            }

            DocumentationUpload document = new DocumentationUpload
            {
                Title = Title,
                Description = Description,
                OriginalFileName = UploadedFile.FileName,
                StoredFileName = storedFileName,
                FilePath = $"/uploads/documentation/{storedFileName}",
                ContentType = UploadedFile.ContentType,
                FileSizeBytes = UploadedFile.Length,
                UploadedBy = User.Identity?.Name,
                UploadedAt = DateTime.Now
            };

            _context.DocumentationUploads.Add(document);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document uploaded successfully.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            DocumentationUpload? document = await _context.DocumentationUploads
                .FirstOrDefaultAsync(d => d.DocumentationUploadId == id);

            if (document == null)
            {
                TempData["ErrorMessage"] = "Document not found.";
                return RedirectToPage();
            }

            string filePath = Path.Combine(
                _environment.WebRootPath,
                document.FilePath.TrimStart('/')
            );

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.DocumentationUploads.Remove(document);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document deleted successfully.";

            return RedirectToPage();
        }

        private async Task LoadDocumentsAsync()
        {
            Documents = await _context.DocumentationUploads
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public string FormatFileSize(long bytes)
        {
            if (bytes < 1024)
            {
                return bytes + " B";
            }

            if (bytes < 1024 * 1024)
            {
                return Math.Round(bytes / 1024.0, 2) + " KB";
            }

            return Math.Round(bytes / 1024.0 / 1024.0, 2) + " MB";
        }
    }
}