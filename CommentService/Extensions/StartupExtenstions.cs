using CommentService.Services.Interfaces;
using CommentService.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace CommentService.Extensions
{
    public static class StartupExtenstions
    {
        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddScoped<ICommentPublisher, RabbitmqCommentPublisher>();
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
            };

            services.AddSingleton<IConnectionFactory>(factory);

            services.AddScoped(c => c.GetService<IConnectionFactory>()!.CreateConnection("Comment service"));

            Task.Run(() =>
            {
                try
                {
                    using var con = factory.CreateConnection("Comment service set-up");
                    using var channel = con.CreateModel();

                    channel.ExchangeDeclare("comments", ExchangeType.Topic);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            });
        }
    }
}
