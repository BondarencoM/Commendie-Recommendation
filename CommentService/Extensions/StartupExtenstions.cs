using CommentService.Services.Interfaces;
using CommentService.Services;
using RabbitMQ.Client;
using System.Threading.RateLimiting;
using System.Security.Claims;

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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        });
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
