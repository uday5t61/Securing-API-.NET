using Microsoft.AspNetCore.Identity;

namespace Identity_WebApp.Account
{
    public class User :IdentityUser
    {
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}
