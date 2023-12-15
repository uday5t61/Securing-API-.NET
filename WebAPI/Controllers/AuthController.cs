using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody]Credential credential)
        {
            if (credential != null && credential.UserName == "admin" && credential.Password == "password")
            {
                //Create claims
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name,"admin"),
                new Claim(ClaimTypes.Email,"admin@myWebsite.com"),
                new Claim("Department","HR"),
                new Claim("Admin","can be anything"),
                new Claim("Manager","Anything"),
                new Claim("EmploymentData","2023-05-31")
                };

                var expiresAt = DateTime.UtcNow.AddSeconds(30);

                return Ok(new
                {
                    access_token = CreateToken(claims,expiresAt),
                    expires_at = expiresAt,
                });
            }

            ModelState.AddModelError("Unauthorize", "Not authorized");
            return Unauthorized(ModelState);
        }

        private string CreateToken(IEnumerable<Claim> claims,DateTime expiresAt) 
        {
            var key = _configuration.GetValue<string>("SecretKey");
            var secretKey = Encoding.ASCII.GetBytes(key ?? string.Empty);

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
