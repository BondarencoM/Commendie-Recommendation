using AuthenticationService.Services;
using AuthenticationService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationService.Extensions
{
    public static class StartupExtenstions
    {
        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(configuration.GetConnectionString("RabbitMq")),
            };

            services.AddSingleton<IConnectionFactory>(factory);

            services.AddScoped(_ => factory.CreateConnection("Authentication service"));

            Task.Run(() =>
            {
                try
                {
                    using var con = factory.CreateConnection("Authentication service set-up");
                    using var channel = con.CreateModel();

                    channel.ExchangeDeclare("users", ExchangeType.Topic, durable: true, autoDelete: false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw;
                }
            });
        }

        public static void UseRabbitMQ(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfiguration>();
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(config.GetConnectionString("RabbitMq")!),
            };

            var con = factory.CreateConnection("Authentication service set-up");

            // DownloadablePersonalData topic
            var channel = con.CreateModel();
            channel.ExchangeDeclare("downloadable-personal-data", type: "topic", durable: true, autoDelete: false);

            const string downloadableDataQueueName = "authentication-service-downloadable-personal-data";

            channel.QueueDeclare(downloadableDataQueueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: downloadableDataQueueName,
                                 exchange: "downloadable-personal-data",
                                 routingKey: "downloadable-personal-data.new");


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += HandleEvent<IDownloadableDataService>;

            channel.BasicConsume(queue: downloadableDataQueueName,
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
                    var logger = scope?.ServiceProvider.GetRequiredService<ILogger<THandler>>();

                    if (logger == null)
                        Console.WriteLine(ex.ToString());
                    else
                        logger.LogError(ex, $"Could not handle event {e} sent by {s}.");
                }
            }
        }
    }
}