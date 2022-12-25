using CommentService.Services.Interfaces;
using CommentService.Services;
using RabbitMQ.Client;
using System.Threading.RateLimiting;
using System.Security.Claims;
using RabbitMQ.Client.Events;

namespace CommentService.Extensions;

public static class StartupExtenstions
{
    public static void AddRabbitMQ(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ICommentPublisher, RabbitmqCommentPublisher>();
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(config.GetConnectionString("RabbitMq")),
        };

        services.AddSingleton<IConnectionFactory>(factory);

        services.AddScoped(c => c.GetService<IConnectionFactory>()!.CreateConnection("Comment service"));

        Task.Run(() =>
        {
            try
            {
                using var con = factory.CreateConnection("Comment service set-up");
                using var channel = con.CreateModel();

                channel.ExchangeDeclare("comments", ExchangeType.Topic, durable: true, autoDelete: false);
                channel.ExchangeDeclare("downloadable-personal-data", type: "topic", durable: true, autoDelete: false);

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

        var con = factory.CreateConnection("Comment service set-up");

        // Users topic
        var channel = con.CreateModel();

        const string downloadableDataQueueName = "comments-service-downloadables";
        
        channel.ExchangeDeclare("users", type: "topic", durable: true, autoDelete: false);

        channel.QueueDeclare(downloadableDataQueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(queue: downloadableDataQueueName,
                             exchange: "users",
                             routingKey: "users.dataRequested");


        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += HandleEvent<IUserService>;

        channel.BasicConsume(queue: downloadableDataQueueName,
                             autoAck: true,
                             consumer: consumer);

        async void HandleEvent<THandler>(object? s, BasicDeliverEventArgs? e) where THandler : IRabbitEventHandler
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
    
    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = 429;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
               RateLimitPartition.GetFixedWindowLimiter(
                   partitionKey: UsernameOrHostname(httpContext),
                   factory: partition => new FixedWindowRateLimiterOptions
                   {
                       PermitLimit = 10,
                       Window = TimeSpan.FromSeconds(1),
                   })
               );

            options.AddPolicy("AddComment", httpContext => RateLimitPartition.GetFixedWindowLimiter(
                UsernameOrHostname(httpContext),
                partition => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 3,
                    Window = TimeSpan.FromMinutes(3)
                }));

            options.OnRejected = (context, cancelation) =>
            {
                ILogger logger = context.HttpContext.RequestServices.GetService<ILogger<RateLimiter>>()!;
                ClaimsPrincipal principal = context.HttpContext.User;
                HttpRequest request = context.HttpContext.Request;

                logger.LogInformation($"User {principal.Identity?.Name ?? "Anonymous"} ({context.HttpContext.Connection.RemoteIpAddress}) " +
                                      $"was rate-limited while trying to {request.Method} {request.Path}");

                return ValueTask.CompletedTask;
            };
        });
    }

    static string UsernameOrHostname(HttpContext context) =>
        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "";
}
