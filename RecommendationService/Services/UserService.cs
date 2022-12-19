using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RecommendationService.Models.User;
using RecommendationService.Services.Interfaces;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecommendationService.Services;

public class UserService : IUserService
{
    private ILogger<CommentService> logger;
    private ICommentService comments;

    public UserService(
       ILogger<CommentService> logger,
       ICommentService comments)
    {
        this.logger = logger;
        this.comments = comments;
    }

    public Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Body.ToArray());
        return args.RoutingKey switch
        {
            "users.delete" => DeleteComment(),
            _ => Default(),
        };

        Task DeleteComment()
        {
            var deleted = JsonSerializer.Deserialize<UserIdentifier> (message);

            var delComments = comments.CleanseUser(deleted.Username);

            return Task.WhenAll(delComments);
        }

        Task Default()
        {
            this.logger.LogWarning("Could not handle Comment message" +
                                    $" with routing key {args.RoutingKey} and body {message}");
            return Task.CompletedTask;
        }
    }

}
