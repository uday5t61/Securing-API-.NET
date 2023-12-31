using Identity_WebApp.Account;
using Identity_WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Identity_WebApp.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public LoginModel(SignInManager<User> signInManager)
        {
            this._signInManager = signInManager;
        }
        [BindProperty]
        public Credentials Credentials { get; set; } = new Credentials();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await _signInManager.PasswordSignInAsync(Credentials.UserName, Credentials.Password, Credentials.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (result.RequiresTwoFactor)
                {
                    //return RedirectToPage("/Account/LoginTwoFactor",
                    //    new
                    //    {
                    //        UserName = this.Credentials.UserName,
                    //        RememberMe = Credentials.RememberMe

                    //    });

                    return RedirectToPage("/Account/LoginTwoFactorWithAuthenticator",
                     new
                     {
                         RememberMe = Credentials.RememberMe

                     });
                    
                }
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "You are locked out");
                }
                else
                {
                    ModelState.AddModelError("Login", "Failed to Login");
                }
            }
            return Page();
        }
    }
}