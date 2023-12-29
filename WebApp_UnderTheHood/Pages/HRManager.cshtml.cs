using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApp_UnderTheHood.DTO;
using WebApp_UnderTheHood.Models;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(policy:"HRManagerOnly")]
    public class HRManagerModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<WeatherForcast> WeatherForcastItems { get; set; } = new List<WeatherForcast>();
        public HRManagerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task OnGetAsync()
        {            
            var strToken = HttpContext.Session.GetString("access_token");
            JwtToken? token;
            if (string.IsNullOrEmpty(strToken))
            {
                token = await Authenticate();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);

                if (token == null || string.IsNullOrWhiteSpace(token.AccessToken)
                    || token.ExpiresAt <= DateTime.UtcNow)
                {
                    token = await Authenticate();
                }
            }

            var httpClient = _httpClientFactory.CreateClient("OurWebAPI");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
            WeatherForcastItems = await httpClient.GetFromJsonAsync<List<WeatherForcast>>("WeatherForecast")?? new List<WeatherForcast>();
        }

        private async Task<JwtToken> Authenticate()
        {
            var httpClient = _httpClientFactory.CreateClient("OurWebAPI");

            var res = await httpClient.PostAsJsonAsync("auth", new Credentials { UserName = "admin", Password = "password" });
            res.EnsureSuccessStatusCode();
            string strJwt = await res.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", strJwt);
            return JsonConvert.DeserializeObject<JwtToken>(strJwt) ?? new JwtToken();
        }
    }
}
