using System.ComponentModel.DataAnnotations;

namespace Identity_WebApp.Models
{
    public class EmailMFA
    {
        [Required]
        [Display(Name ="Security Code")]
        public string SecurityCode { get; set; } = string.Empty;    
        public bool RememberMe { get; set; }
    }
}
