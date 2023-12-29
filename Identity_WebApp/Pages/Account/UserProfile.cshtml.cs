using Identity_WebApp.Account;
using Identity_WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Identity_WebApp.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public UserProfileViewModel  UserProfile { get; set; }

        [BindProperty]
        public string? SuccessMessage { get; set; }
        public UserProfileModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.UserProfile = new UserProfileViewModel();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();         

            if (user != null)
            { 
                this.UserProfile.Email = User.Identity?.Name ?? string.Empty;
                this.UserProfile.Department = departmentClaim?.Value??string.Empty;
                this.UserProfile.Position = positionClaim?.Value ?? string.Empty;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();

            try
            {
                if (user != null && departmentClaim != null)
                {
                    await userManager.ReplaceClaimAsync(user, departmentClaim, new Claim(departmentClaim.Type, UserProfile.Department));
                }
                if (user != null && positionClaim != null)
                {
                    await userManager.ReplaceClaimAsync(user, positionClaim, new Claim(positionClaim.Type, UserProfile.Position));
                }
            }
            catch
            {
                ModelState.AddModelError("UserProfile", "Error in updating user profile");
            }

            SuccessMessage = "Profile is successfully updated";

            return Page();
        }

        private async Task<(User? user, Claim? departmentclaim, Claim? positionClaim)> GetUserInfoAsync()
        {
            var user = await userManager.FindByNameAsync(User.Identity?.Name ?? string.Empty);

            if (user != null)
            {
                var claims = await userManager.GetClaimsAsync(user);

                var departmentClaim = claims.FirstOrDefault(x => x.Type == "Department");
                var positionClaim = claims.FirstOrDefault(x => x.Type == "Postion");

                return (user,departmentClaim, positionClaim);
            }

            return (null, null, null);
        }
    }
}
