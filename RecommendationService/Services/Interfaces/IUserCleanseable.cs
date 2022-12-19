using RabbitMQ.Client.Events;
using RecommendationService.Models.Comments;
using RecommendationService.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IUserCleanseable
{
    public Task CleanseUser(UserIdentifier user);
}
