using Identity_WebApp.Account;
using Identity_WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity_WebApp.Pages.Account
{
    public class LoginTwoFactorModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        [BindProperty]
        public EmailMFA EmailMFA { get; set; }
        public LoginTwoFactorModel(UserManager<User> userManager,SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            EmailMFA = new EmailMFA();
        }
        public async Task OnGetAsync(string userName,bool rememberMe)
        {
            var user = await userManager.FindByNameAsync(userName);
            EmailMFA.SecurityCode = string.Empty;
            EmailMFA.RememberMe = rememberMe;

            if (user != null)
            {
                var securityCode = await userManager.GenerateTwoFactorTokenAsync(user,"Email");

                Console.WriteLine($"Security code:{securityCode}");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var result = await signInManager.TwoFactorSignInAsync("Email", EmailMFA.SecurityCode, EmailMFA.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login2FA", "You are locked out");
                }
                else
                {
                    ModelState.AddModelError("Login2FA", "Failed to Login");
                }
            }
            return Page();
        }
    }
}
