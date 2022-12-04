using AuthenticationService.Models.Messages;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public interface IUserPublisher
    {
        Task Created(UserCreatedMessage user);
    }
}
