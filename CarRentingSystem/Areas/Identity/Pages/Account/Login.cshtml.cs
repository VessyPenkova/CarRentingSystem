#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CarRentingSystem.Infrastucture.Data;

namespace CarRentingSystem.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        
        public LoginModel(SignInManager<User> _signInManager)
        {
            signInManager = _signInManager;      
        }

        [BindProperty]
        public InputModel Input { get; set; }//

     
        public IList<AuthenticationScheme> ExternalLogins { get; set; }//

       
        public string ReturnUrl { get; set; }//

      
        [TempData]
        public string ErrorMessage { get; set; }//

      
        public class InputModel
        {
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }//

          
            [Required]
            [DataType(DataType.Password)]//
            public string Password { get; set; }//

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }//
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await signInManager
                .GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(Input.Email, Input.Password,
                    Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }
    }
}