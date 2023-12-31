using Identity_WebApp.Account;
using Identity_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;

namespace Identity_WebApp.Pages.Account
{
    [Authorize]
    public class AuthenticatorWithMFASetupModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public AuthenticatorSetupMFAViewModel KeyViewModel { get; set; }
        [BindProperty]
        public bool Succeeded { get; set; }
        public AuthenticatorWithMFASetupModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            KeyViewModel = new AuthenticatorSetupMFAViewModel();
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
                KeyViewModel.Key = key ?? string.Empty;

                KeyViewModel.QRCodeBytes = GenerateQrCodeBytes("myApp",
                    KeyViewModel.Key,
                    user.Email ?? string.Empty
                    );
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await userManager.GetUserAsync(base.User);

            if (user != null && await userManager.VerifyTwoFactorTokenAsync(
                user,
                userManager.Options.Tokens.AuthenticatorTokenProvider,
                KeyViewModel.SecurityCode))
            {
                await userManager.SetTwoFactorEnabledAsync(user, true);
                Succeeded = true;
            }
            else
            {
                ModelState.AddModelError("AuthenticatorSetup", "Something went wrong with authenticator setup.");
                Succeeded = false;
            }
            return Page();
        }

        private byte[] GenerateQrCodeBytes(string provider,string key,string userEmail)
        {
            var qrcodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrcodeGenerator.CreateQrCode(
                $"otpauth://totp/{provider}:{userEmail}?secret={key}&issuer={provider}",
                QRCodeGenerator.ECCLevel.Q
                );

            var qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }
    }
}
