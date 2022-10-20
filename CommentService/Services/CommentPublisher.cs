using CommentService.Models;
using CommentService.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommentService.Services
{
    public class RabbitmqCommentPublisher : ICommentPublisher, IDisposable
    {
        private readonly IConnection connection;
        private readonly ILogger<RabbitmqCommentPublisher> logger;
        private readonly IModel channel;

        public RabbitmqCommentPublisher(IConnection connection, ILogger<RabbitmqCommentPublisher> logger)
        {
            this.connection = connection;
            this.logger = logger;
            this.channel = connection.CreateModel();
        }

        public void Dispose()
        {
            this.channel.Dispose();
        }

        public Task Publish(Comment input)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(input));
            channel.BasicPublish(exchange: "comments",
                                 routingKey: $"comments.{input.Domain}.new",
                                 basicProperties: null,
                                 body: body);
            return Task.CompletedTask;
        }
    }
}
