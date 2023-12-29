using System.ComponentModel.DataAnnotations;

namespace Identity_WebApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}
