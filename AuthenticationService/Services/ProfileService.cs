using AuthenticationService.Models;
using AuthenticationService.Models.DTO;
using AuthenticationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public class ProfileService : IProfileService
    {
        private HttpClient _http;
        public ProfileService(HttpClient http)
        {
            _http = http;
        }

        public Task<HttpResponseMessage> NotifyOfNewUser(ApplicationUser user)
        {
            return _http.PostAsJsonAsync("profiles", new UserProfilleDTO(user));
        }


    }
}
