using Identity_WebApp.Account;
using Identity_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity_WebApp.Pages.Account
{
    [Authorize]
    public class AuthenticatorWithMFASetupModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public SetupAuthenticatorMFAViewModel KeyViewmodel { get; set; }
        public AuthenticatorWithMFASetupModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            KeyViewmodel = new SetupAuthenticatorMFAViewModel();
        }
        public async Task OnGetAsync()
        {
            var user = await userManager.GetUserAsync(base.User);

            if (user != null)
            {
                await userManager.ResetAuthenticatorKeyAsync(user);

                var key = await userManager.GetAuthenticatorKeyAsync(user);

                //if(string.IsNullOrEmpty(key)) 
                //{
                //    await userManager.ResetAuthenticatorKeyAsync(user);
                //    key = await userManager.GetAuthenticatorKeyAsync(user);
                //}
                KeyViewmodel.Key = key ?? string.Empty;
            }

        }
    }
}
