using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace CommentService.Services.Interfaces
{
    public interface IRabbitEventHandler
    {
        Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args);
    }
}