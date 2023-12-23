using Identity_WebApp.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity_WebApp.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public string Message { get; set; } = string.Empty;
        public ConfirmEmailModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(string userId,string token)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if(user!= null)
            {
                var result = await this.userManager.ConfirmEmailAsync(user, token);

                if(result.Succeeded)
                {
                    Message = "Email address is successfully confirmed. Please login now!";
                    return Page();
                }
            }
            this.Message = "Failed to Validate the email";
            return Page();
        }
    }
}
