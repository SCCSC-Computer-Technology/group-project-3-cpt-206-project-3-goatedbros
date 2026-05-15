// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PantryPlatoonMVCMain.Data;
using PantryPlatoonMVCMain.Models;

namespace PantryPlatoonMVCMain.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public SelectList CampusList { get; set; } = new SelectList(new List<Campus>(), "CampusId", "CampusName");
        public StaticPage RulesPage { get; set; }
        public StaticPage LiabilityPage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 

            /// <summary>
            ///     SCC Student ID
            /// </summary>
            [Required]
            [StringLength(7)]
            [RegularExpression(@"^\d{7}$", ErrorMessage = "SCC ID must be 7 digits.")]
            [Display(Name = "SCC ID")]
            public string SCCId { get; set; }

            /// <summary>
            ///     Student First Name
            /// </summary>
            [Required]
            [StringLength(100)]
            [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "Only letters, spaces, apostrophes, and hyphens are allowed.")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            /// <summary>
            ///     Student Last Name
            /// </summary>
            [Required]
            [StringLength(100)]
            [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "Only letters, spaces, apostrophes, and hyphens are allowed.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Campus")]
            public int? CampusId { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the pantry rules to register.")]
            [Display(Name = "I have read and agree to the pantry rules")]
            public bool AcceptRules { get; set; }

            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the liability waiver to register.")]
            [Display(Name = "I have read and agree to the liability waiver")]
            public bool AcceptLiability { get; set; }
            [Required]
            [Range(16, 100)]
            [Display(Name = "Age")]
            public int? Age { get; set; }

            [Required]
            [Display(Name = "Adults in Household")]
            public int? AdultsInHousehold { get; set; }

            [Required]
            [Display(Name = "Children 0-5")]
            public int? ChildrenUnder5 { get; set; }

            [Required]
            [Display(Name = "Children 5-18")]
            public int? Children5To18 { get; set; }

            [Required]
            [Display(Name = "Student Status")]
            public string StudentStatus { get; set; }

            [Required]
            [Display(Name = "Employment Status")]
            public string EmploymentStatus { get; set; }

            [Display(Name = "Household Employment Status")]
            public string HouseholdEmploymentStatus { get; set; }

            [Required]
            [Display(Name = "Benefits Status")]
            public string BenefitsStatus { get; set; }

            [Required]
            [Display(Name = "Kitchen Access")]
            public string KitchenAccess { get; set; }

            [Required]
            [Display(Name = "Dietary Restrictions")]
            public bool HasDietaryRestrictions { get; set; }

            [Display(Name = "Dietary Restriction Explanation")]
            public string DietaryRestrictionExplanation { get; set; }

            [Display(Name = "Specialty Items Needed")]
            public string SpecialtyItemsNeeded { get; set; }

            [Display(Name = "Specialty Items Explanation")]
            public string SpecialtyItemsExplanation { get; set; }

            [Display(Name = "Additional Notes")]
            public string AdditionalNotes { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            await LoadCampusListAsync();
            await LoadStaticPagesAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            await LoadCampusListAsync();
            await LoadStaticPagesAsync();

            // Check if SCCId already exists
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.SCCId == Input.SCCId);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this SCC ID already exists.");
            }

            if (ModelState.IsValid)
            {
                if (!Input.Email.EndsWith("@sccsc.edu", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("Input.Email", "You must use your official school email to register.");
                    return Page();
                }

                var user = CreateUser();

                user.SCCId = Input.SCCId;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.CampusId = Input.CampusId;
                user.Age = Input.Age;
                user.AdultsInHousehold = Input.AdultsInHousehold;
                user.ChildrenUnder5 = Input.ChildrenUnder5;
                user.Children5To18 = Input.Children5To18;
                user.StudentStatus = Input.StudentStatus;
                user.EmploymentStatus = Input.EmploymentStatus;
                user.HouseholdEmploymentStatus = Input.HouseholdEmploymentStatus;
                user.BenefitsStatus = Input.BenefitsStatus;
                user.KitchenAccess = Input.KitchenAccess;
                user.HasDietaryRestrictions = Input.HasDietaryRestrictions;
                user.DietaryRestrictionExplanation = Input.DietaryRestrictionExplanation;
                user.SpecialtyItemsNeeded = Input.SpecialtyItemsNeeded;
                user.SpecialtyItemsExplanation = Input.SpecialtyItemsExplanation;
                user.AdditionalNotes = Input.AdditionalNotes;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    await _userManager.AddToRoleAsync(user, "User");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private async Task LoadCampusListAsync()
        {
            var campuses = await _context.Campuses
                .OrderBy(c => c.CampusName)
                .ToListAsync();

            CampusList = new SelectList(campuses, "CampusId", "CampusName");
        }

        private async Task LoadStaticPagesAsync()
        {
            RulesPage = await _context.StaticPages
                .FirstOrDefaultAsync(p => p.PageName == "RulesForm");

            LiabilityPage = await _context.StaticPages
                .FirstOrDefaultAsync(p => p.PageName == "LiabilityForm");
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
