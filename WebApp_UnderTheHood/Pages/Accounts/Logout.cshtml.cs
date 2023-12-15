using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages.Accounts
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync() 
        {
            await HttpContext.SignOutAsync("MyAuthCookie");
            return RedirectToPage("/Index");
        }
    }
}
