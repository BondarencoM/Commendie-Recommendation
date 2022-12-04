using ProfileService.Common;
using RabbitMQ.Client.Events;

namespace ProfileService.Profiles;

public interface IProfileService : IRabbitEventHandler
{
    Task<Profile> FindByUsername(string username);
}
