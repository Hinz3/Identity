using Common.DTOs;
using DemoWebApp.Models.Requests;
using IdentitySystem.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet(string code, string state)
        {
            if (code == null)
            {
                return;
            }

            var httpClient = new HttpClient();
            var body = new TokenRequest
            {
                GrantType = "authorization_code",
                Code = code,
                ClientId = "D7B520E2-3E2B-479A-BEE0-93DCBA7B44E2",
                ClientSecret = "test"
            };

            var response = await httpClient.PostAsJsonAsync("https://localhost:7112/api/v1/Token", body);
            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<TokenResponseDTO>>();
        }
    }
}