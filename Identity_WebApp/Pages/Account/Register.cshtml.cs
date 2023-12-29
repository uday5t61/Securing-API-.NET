using Identity_WebApp.Account;
using Identity_WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Identity_WebApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public RegisterModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; } = new RegisterViewModel();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            //Validate Email Address(optional as it is already handled in startup)

            //Create User

            var user = new User
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email,
                //Department = RegisterViewModel.Department,
                //Position = RegisterViewModel.Position
                
            };

            var departmentClaim = new Claim("Department", RegisterViewModel.Department);
            var positionClaim = new Claim("Postion", RegisterViewModel.Position);

            var result = await _userManager.CreateAsync(user,RegisterViewModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, departmentClaim);
                await _userManager.AddClaimAsync(user, positionClaim);

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values: new { userId = user.Id, token = confirmationToken }) ?? "");

               // return RedirectToPage("/Account/Login");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return Page();
            }
             

        }
    }
}
