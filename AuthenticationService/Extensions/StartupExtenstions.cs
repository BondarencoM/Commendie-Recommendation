using AuthenticationService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationService.Extensions
{
    public static class StartupExtenstions
    {
        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserPublisher, RabbitmqUserPublisher>();
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
    }
}