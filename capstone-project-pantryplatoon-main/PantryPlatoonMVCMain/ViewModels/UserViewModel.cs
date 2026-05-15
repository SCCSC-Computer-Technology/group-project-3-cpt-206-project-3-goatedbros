using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.ViewModels
{

    public class UserViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; } = null!;
        public IEnumerable<IdentityRole> Roles { get; set; } = null!;
    }
}
