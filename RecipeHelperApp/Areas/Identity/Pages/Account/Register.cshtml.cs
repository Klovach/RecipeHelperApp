// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using RecipeHelperApp.Data;
using RecipeHelperApp.Models;

namespace RecipeHelperApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
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

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Birth Date")]
            public DateTime BirthDate { get; set; }

            [Required]
            public string Sex{ get; set; }

            [Required(ErrorMessage = "Height is required")]
            public decimal Height
            {
                get
                {
                    return convertToCM(Feet, Inches);
                }
                set
                {
                    Height = Height;
                }
            }


            [Required]
            [Range(4, 8, ErrorMessage = "Height must be between 4 and 8 feet.")]
            public int Feet { get; set; }

            [Required]
            [Range(0, 11, ErrorMessage = "Inches must be between 0 and 11 inches.")]
            public int Inches { get; set; }

            [Required]
            [Range(65, 1000, ErrorMessage = "Weight must be between 65 and 1000 lbs.")]
            public decimal Weight { get; set; }

            [Required]
            [Display(Name = "Target Weight")]
            public decimal TargetWeight { get; set; }

            [Required]
            [Display(Name = "Target Weight Date")]
            public DateTime TargetWeightDate { get; set; }

            [Required]
            [Display(Name = "Activity Level")]
            public string ActivityLevel { get; set; }

            [Required]
            [Display(Name = "Fitness Goal")]
            public string FitnessGoal { get; set; }

           

            /// <summary>
            /// Returns values in cm. 
            /// </summary>
            /// <param name="inches"></param>
            /// <param name="feet"></param>
            /// <returns> centimiters </returns>
            public decimal convertToCM(decimal inches, decimal feet)
            {
                decimal cm;
                decimal totalInches = feet * 12 + inches;
                cm = totalInches * 2.54m; 
                return cm;
            }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                   
                    BirthDate = Input.BirthDate,
                    Sex = Input.Sex,
                    Height = Input.Height,
                    Weight = Input.Weight,
                    TargetWeight = Input.TargetWeight,
                    TargetWeightDate = Input.TargetWeightDate,
                    ActivityLevel = Input.ActivityLevel,
                    FitnessGoal = Input.FitnessGoal
                };

                // Create WeekModel : Meal Plan Manager For User 
                user.WeekModel = new List<Week>(); 
                Week week = new Week("My First Week");
                user.WeekModel.Add(week); 

                // Create Default Nutritional Form : Nutritional Settings For User 
                Console.WriteLine("Called NutritionForm Create");
                user.NutritionFormModel = new NutritionForm(user.BirthDate, user.Sex, user.Height, user.Weight, user.TargetWeight, user.TargetWeightDate, user.ActivityLevel, user.FitnessGoal);

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    // CHANGE EMAIL CONFIRMATION MESSAGE HERE: 
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        // $"Please confirm your account by <a href='" + (HtmlEncoder.Default.Encode(callbackUrl)) + "'>clicking here</a>.");
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
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
