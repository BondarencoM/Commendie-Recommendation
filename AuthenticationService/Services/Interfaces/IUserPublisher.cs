using AuthenticationService.Data.Messages;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Interfaces
{
    public interface IUserPublisher
    {
        Task Created(UserIdentifierMessage user);
        Task Deleted(UserIdentifierMessage user);
        Task DataRequested(UserIdentifierMessage user);

    }
}
