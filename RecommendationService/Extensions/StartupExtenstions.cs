using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var channel = con.CreateModel();

            channel.ExchangeDeclare(exchange: "comments", type: "topic", durable: true, autoDelete: false);

            const string queueName = "recommendation-service-comments";

            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: queueName,
                                 exchange: "comments",
                                 routingKey: "comments.recommendation.#");
            channel.QueueBind(queue: queueName,
                                 exchange: "comments",
                                 routingKey: "comments.delete");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (s, e) =>
            {
                using var scope = app.ApplicationServices.CreateScope();

                var handler = scope.ServiceProvider.GetService<ICommentService>();
                await handler.HandleAsyncEvent(s, e);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
