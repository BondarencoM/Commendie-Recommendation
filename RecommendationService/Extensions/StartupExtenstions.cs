using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RecommendationService.Services.Interfaces;
using System;

namespace RecommendationService.Extensions
{
    public static class StartupExtenstions
    {
        public static void UseRabbitMQ(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfiguration>();
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(config.GetConnectionString("RabbitMq")),
            };

            var con = factory.CreateConnection("Recommendation service set-up");

            // Comments topic
            var channel = con.CreateModel();

            channel.ExchangeDeclare(exchange: "comments", type: "topic", durable: true, autoDelete: false);

            const string recommendationCommentsQueue = "recommendation-service-comments";

            channel.QueueDeclare(recommendationCommentsQueue, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: recommendationCommentsQueue,
                                 exchange: "comments",
                                 routingKey: "comments.recommendation.#");
            channel.QueueBind(queue: recommendationCommentsQueue,
                                 exchange: "comments",
                                 routingKey: "comments.delete");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += HandleEvent<ICommentService>;

            channel.BasicConsume(queue: recommendationCommentsQueue,
                                 autoAck: true,
                                 consumer: consumer);

            // Users topic
            const string usersQueueName = "recommendation-service-users";
            channel = con.CreateModel();
            channel.ExchangeDeclare(exchange: "users", type: "topic", durable: true, autoDelete: false);


            channel.QueueDeclare(usersQueueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: usersQueueName,
                                 exchange: "users",
                                 routingKey: "users.delete");


            consumer = new EventingBasicConsumer(channel);
            consumer.Received += HandleEvent<IUserService>;

            channel.BasicConsume(queue: usersQueueName,
                                 autoAck: true,
                                 consumer: consumer);

            async void HandleEvent<THandler>(object s, BasicDeliverEventArgs e) where THandler : IRabbitEventHandler
            {
                IServiceScope scope = null;
                try
                {
                    scope = app.ApplicationServices.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<THandler>();
                    await service.HandleAsyncEvent(s, e);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<THandler>>();
                    
                    logger.LogError(ex, $"Could not handle event {e} sent by {s}.");
                }
                finally
                {
                    scope.Dispose();
                }

            }
        }
    }
}
