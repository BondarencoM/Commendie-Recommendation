using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Interfaces
{
    public interface IProfileService
    {
        Task<HttpResponseMessage> NotifyOfNewUser(ApplicationUser user);
    }
}
