using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces
{
    public interface IRabbitEventHandler
    {
        Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args);
    }
}