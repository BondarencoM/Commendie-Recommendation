using AuthenticationService.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Interfaces
{
    public interface IProfileService
    {
        Task NotifyOfNewUser(ApplicationUser user);
    }
}
