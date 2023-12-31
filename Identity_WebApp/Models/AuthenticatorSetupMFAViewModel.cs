using System.ComponentModel.DataAnnotations;

namespace Identity_WebApp.Models
{
    public class AuthenticatorSetupMFAViewModel
    {
        public string? Key { get; set; } = string.Empty;
        [Required]
        [Display(Name ="Code")]
        public string SecurityCode { get; set; } = string.Empty;

        [Required]
        public byte[] QRCodeBytes { get; set; } 
    }
}
