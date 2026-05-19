using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.Util
{
    public static class StaticPageHelper
    {

        public static void GenerateStaticPages(WebApplication app, bool forceCreate)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            if (forceCreate)
            {
                context.StaticPages.RemoveRange(context.StaticPages);
                context.SaveChanges();
            }

            if (!context.StaticPages.Any())
            {
                var templatesPath = Path.Combine(AppContext.BaseDirectory, "StaticPageTemplates");

                string ReadTemplate(string fileName, string? fallback = null)
                {
                    var path = Path.Combine(templatesPath, fileName);
                    return File.Exists(path) ? File.ReadAllText(path) : fallback ?? $"<p>Template {fileName} not found.</p>";
                }

                context.StaticPages.AddRange(
                    new StaticPage { PageName = "Home", HtmlContent = ReadTemplate("Home.html", "Welcome to the SCC Pantry!</h1><p>Your campus food pantry supporting students in need (this is a placeholder).</p>") },
                    new StaticPage { PageName = "About", HtmlContent = ReadTemplate("About.html", "<h1>About Us</h1><p>Learn about our mission to support students (this is a placeholder).</p>") },
                    new StaticPage { PageName = "Contact", HtmlContent = ReadTemplate("Contact.html", "<h1>Contact Us</h1><p>Get in touch with our team (this is a placeholder).</p>") },
                    new StaticPage { PageName = "VisitConfirmation", HtmlContent = ReadTemplate("VisitConfirmation.html", "<p>Visit confirmation instructions go here. (this is a placeholder).</p>") },
                    new StaticPage { PageName = "RulesForm", HtmlContent = ReadTemplate("RulesForm.html", "<p>Rules go here. (this is a placeholder).</p>") },
                    new StaticPage { PageName = "LiabilityForm", HtmlContent = ReadTemplate("LiabilityForm.html", "<p>Liability information goes here. (this is a placeholder).</p>") }
                );

                context.SaveChanges();
            }

        }
    }
}
