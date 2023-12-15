using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApp_UnderTheHood.Models;

namespace WebApp_UnderTheHood.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credentials Credentials { get; set; } = new Credentials();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (Credentials != null && Credentials.UserName == "admin" && Credentials.Password == "password")
            {
                //Create claims
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name,"admin"),
                new Claim(ClaimTypes.Email,"admin@myWebsite.com"),
                new Claim("Department","HR"),
                new Claim("Admin","can be anything"),
                new Claim("Manager","Anything"),
                new Claim("EmploymentData","2023-05-31")
                };

                var identity = new ClaimsIdentity(claims, "MyAuthCookie");

                var claimPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credentials.RememberMe
                };

                await HttpContext.SignInAsync("MyAuthCookie", claimPrincipal, authProperties);

                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}