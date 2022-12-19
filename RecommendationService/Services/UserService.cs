using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RecommendationService.Models.User;
using RecommendationService.Services.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecommendationService.Services;

public class UserService : IUserService
{
    private ILogger<CommentService> logger;
    private readonly IEnumerable<IUserCleanseable> servicesWithUsers;

    public UserService(
       ILogger<CommentService> logger,
       IEnumerable<IUserCleanseable> servicesWithUsers)
    {
        this.logger = logger;
        this.servicesWithUsers = servicesWithUsers;
    }

    public Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Body.ToArray());
        return args.RoutingKey switch
        {
            "users.delete" => CleanseUserFromEverywhere(),
            _ => Default(),
        };

        Task CleanseUserFromEverywhere()
        {
            var deleted = JsonSerializer.Deserialize<UserIdentifier> (message);

            var tasks = this.servicesWithUsers
                            .Select(s => s.CleanseUser(deleted));

            return Task.WhenAll(tasks);
        }

        Task Default()
        {
            this.logger.LogWarning("Could not handle Comment message" +
                                    $" with routing key {args.RoutingKey} and body {message}");
            return Task.CompletedTask;
        }
    }

}
