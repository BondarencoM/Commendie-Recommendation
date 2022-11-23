using CommentService.Models;
using CommentService.Models.Messages;
using CommentService.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CommentService.Services;

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

    public Task Created(Comment input)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(input));
        channel.BasicPublish(exchange: "comments",
                             routingKey: $"comments.{input.Domain}.new",
                             basicProperties: null,
                             body: body);
        return Task.CompletedTask;
    }

    public Task Deleted(DeleteCommentMessage input)
    {
        // indicate the domain if known, skip otherwise
        string domain = input.Domain is null ? "" : "." + input.Domain;
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(input));
        channel.BasicPublish(exchange: "comments",
                             routingKey: $"comments{domain}.delete",
                             basicProperties: null,
                             body: body);
        return Task.CompletedTask;
    }

    public Task Edited(EditCommentMessage newComment)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(newComment));
        channel.BasicPublish(exchange: "comments",
                             routingKey: $"comments.{newComment.Domain}.edit",
                             basicProperties: null,
                             body: body);
        return Task.CompletedTask;
    }
}
