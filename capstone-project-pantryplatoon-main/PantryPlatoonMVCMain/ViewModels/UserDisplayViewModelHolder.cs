using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace PantryPlatoonMVCMain.ViewModels
{

    public class UserDisplayViewModelHolder
    {
        public IEnumerable<UserDisplayViewModel> Users { get; set; } = null!;
        public IEnumerable<IdentityRole> Roles { get; set; } = null!;
    }
}
