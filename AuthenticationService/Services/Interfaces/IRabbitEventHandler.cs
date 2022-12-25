using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Interfaces
{
    public interface IRabbitEventHandler
    {
        Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args);
    }
}