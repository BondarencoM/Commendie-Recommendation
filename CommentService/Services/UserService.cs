using CommentService.Exceptions;
using CommentService.Extensions;
using CommentService.Models;
using CommentService.Models.Messages;
using CommentService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using System.Security.Principal;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data;
using System.Threading.Channels;
using RabbitMQ.Client;

namespace CommentService.Services;

public class UserService : IUserService
{
    private readonly IEnumerable<IHasDownloadableUserData> dataProviders;
    private readonly ILogger<CommentService> logger;
    private readonly IModel channel;


    public UserService(
        IEnumerable<IHasDownloadableUserData> dataProviders,
        IConnection connection,
        ILogger<CommentService> logger)
    {
        this.channel = connection.CreateModel();
        this.dataProviders = dataProviders;
        this.logger = logger;
    }

    public Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Body.ToArray());
        return args.RoutingKey switch
        {
            "users.dataRequested" => SendUserData(),
            _ => Default(),
        };

        async Task SendUserData()
        {
            var user = JsonSerializer.Deserialize<UserIdentifierIM>(message)
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(UserIdentifierIM)} from {message}");

            var allData = new Dictionary<string, object>();
            foreach (var prov in this.dataProviders)
            {
                try
                {
                    var dataPoint = await prov.GetDownloadableUserData(user.Username);
                    allData[dataPoint.Name] = dataPoint.JsonData;
                }
                catch(Exception ex)
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
            this.logger.LogWarning("Could not handle message" +
                                    $" with routing key {args.RoutingKey} and body {message}");
            return Task.CompletedTask;
        }
    }
}
