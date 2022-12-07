using ProfileService.Comments;
using ProfileService.Common;
using ProfileService.Profiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProfileService.Extensions;

public static class StartupExtenstions
{
    public static void UseRabbitMQ(this IApplicationBuilder app)
    {
        var config = app.ApplicationServices.GetService<IConfiguration>();
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(config.GetConnectionString("RabbitMq")!),
        };

        var con = factory.CreateConnection("Profile service set-up");

        // Comments topic
        var channel = con.CreateModel();
        channel.ExchangeDeclare(exchange: "comments", type: "topic", durable: true, autoDelete: false);

        const string commentQueueName = "profile-service-comments";

        channel.QueueDeclare(commentQueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(queue: commentQueueName,
                             exchange: "comments",
                             routingKey: "comments.#.#");


        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += HandleEvent<ICommentService>;

        channel.BasicConsume(queue: commentQueueName,
                             autoAck: true,
                             consumer: consumer);


        // Users topic
        const string usersQueueName = "profile-service-users";
        channel = con.CreateModel();
        channel.ExchangeDeclare(exchange: "users", type: "topic", durable: true, autoDelete: false);


        channel.QueueDeclare(usersQueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(queue: usersQueueName,
                             exchange: "users",
                             routingKey: "users.#");


        consumer = new EventingBasicConsumer(channel);
        consumer.Received += HandleEvent<IProfileService>;

        channel.BasicConsume(queue: usersQueueName,
                             autoAck: true,
                             consumer: consumer);

        async void HandleEvent<THandler>(object? s, BasicDeliverEventArgs e) where THandler : IRabbitEventHandler
        { 
            ILogger<THandler>? logger = null;
            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                logger = scope.ServiceProvider.GetRequiredService<ILogger<THandler>>();
                var service = scope.ServiceProvider.GetRequiredService<THandler>();
                await service.HandleAsyncEvent(s, e);
            }
            catch (Exception ex)
            {
                if (logger == null)
                    Console.WriteLine(ex.ToString());
                else
                    logger.LogError(ex, $"Could not handle event {e} sent by {s}.");
            }
            
        }
    }
}
