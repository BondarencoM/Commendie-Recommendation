using AuthenticationService.Models;
using AuthenticationService.Models.DTO;
using AuthenticationService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public class ProfileService : IProfileService
    {
        private HttpClient _http;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(HttpClient http, ILogger<ProfileService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task NotifyOfNewUser(ApplicationUser user)
        {
            try
            {
                await _http.PostAsJsonAsync("profiles", new UserProfilleDTO(user));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Could not notify profile service");
            }
        }


    }
}
