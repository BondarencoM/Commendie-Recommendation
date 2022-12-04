using RabbitMQ.Client.Events;

namespace ProfileService.Common
{
    public interface IRabbitEventHandler
    {
        Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args);
    }
}