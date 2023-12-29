using System.ComponentModel.DataAnnotations;

namespace Identity_WebApp.Models
{
    public class UserProfileViewModel
    {
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Department { get; set; } = string.Empty;
        [Required]
        public string Position { get; set; } = string.Empty;
    }
}
