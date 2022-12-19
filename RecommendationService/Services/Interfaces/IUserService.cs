using RabbitMQ.Client.Events;
using RecommendationService.Models.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IUserService : IRabbitEventHandler
{
}
