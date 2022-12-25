using CommentRecommendationService.Models.User;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RecommendationService.Models.User;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RecommendationService.Services;

public class UserService : IUserService
{
    private IModel channel;
    private ILogger<CommentService> logger;
    private readonly IEnumerable<IHasUserData> servicesWithUsers;

    public UserService(
       ILogger<CommentService> logger,
        IConnection connection,
       IEnumerable<IHasUserData> servicesWithUsers)
    {
        this.channel = connection.CreateModel();
        this.logger = logger;
        this.servicesWithUsers = servicesWithUsers;
    }

    public Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Body.ToArray());
        return args.RoutingKey switch
        {
            "users.deleted" => CleanseUserFromEverywhere(),
            "users.dataRequested" => SendDownloadablePersonalData(),
            _ => Default(),
        };

        async Task CleanseUserFromEverywhere()
        {
            var deleted = JsonSerializer.Deserialize<UserIdentifierIM> (message)
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(UserIdentifierIM)} from {message}");

            // We could execute all the cleansing in parallel, but that will open
            // many simultaneous connections to the DB which will slow down other services
            // while this service is not in a hurry to complete because no one is waiting on it.
            var exceptions = new List<Exception>();
            foreach (var service in this.servicesWithUsers)
            {
                try
                {
                    await service.CleanseUser(deleted);
                }
                catch(Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Any()) throw new AggregateException(exceptions);
        }

        async Task SendDownloadablePersonalData()
        {
            var user = JsonSerializer.Deserialize<UserIdentifierIM>(message)
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(UserIdentifierIM)} from {message}");

            var allData = new Dictionary<string, object>();
            foreach (var prov in this.servicesWithUsers)
            {
                try
                {
                    var dataPoint = await prov.GetDownloadableUserData(user);
                    allData[dataPoint.Name] = dataPoint.JsonData;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Could not get data from " + prov.GetType().FullName);
                }
            }

            var dataMessage = new DownloadablePersonalDataMessage(allData, user.Username);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dataMessage));
            channel.BasicPublish(exchange: "downloadable-personal-data",
                                 routingKey: $"downloadable-personal-data.new",
                                 basicProperties: null,
                                 body: body);
        }
        
        Task Default()
        {
            this.logger.LogWarning("Could not handle Comment message" +
                                    $" with routing key {args.RoutingKey} and body {message}");
            return Task.CompletedTask;
        }
    }

}
