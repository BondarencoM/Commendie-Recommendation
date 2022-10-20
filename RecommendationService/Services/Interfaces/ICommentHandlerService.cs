using RabbitMQ.Client.Events;
using RecommendationService.Models.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces
{
    public interface ICommentService
    {
        Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args);

        Task<List<Comment>> GetCommentsForRecommendation(long id, int limit, int skip);
    }
}
