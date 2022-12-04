using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProfileService.Profiles;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProfileService.Comments;

public class CommentService : ICommentService
{
    private readonly ILogger<CommentService> logger;
    private readonly DatabaseContext db;

    public CommentService(
        ILogger<CommentService> logger,
        DatabaseContext db)
    {
        this.logger = logger;
        this.db = db;
    }

    public Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Body.ToArray());

        return args.RoutingKey.EndsWith(".new") ? AddComment()
            : args.RoutingKey.EndsWith(".delete") ? DeleteComment()
            : args.RoutingKey.EndsWith(".edit") ? EditComment()
            : Default();

        async Task AddComment()
        {
            var newComment = JsonSerializer.Deserialize<CreateCommentIM>(message)
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(CreateCommentIM)} from {message}");

            if (newComment.Username is null or "Anonymous") { }

            var comment = new Comment(newComment);
            this.db.Comments.Add(comment);

            await this.db.SaveChangesAsync();
        }

        async Task DeleteComment()
        {
            var deleted = JsonSerializer.Deserialize<DeleteCommentIM>(message)
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(DeleteCommentIM)} from {message}");

            await db.Comments
                .Where(c => c.Id == deleted.Id)
                .ExecuteUpdateAsync(c => 
                    c.SetProperty(p => p.Text, "[removed]")
                     .SetProperty(p => p.IsDeleted, true)
                );
        }

        async Task EditComment()
        {
            var edited = JsonSerializer.Deserialize<EditCommentIM>(message)
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(EditCommentIM)} from {message}");

            var fromDb = await db.Comments.FindAsync(edited.Id);

            await db.Comments
               .Where(c => c.Id == edited.Id)
               .ExecuteUpdateAsync(c =>
                   c.SetProperty(p => p.Text, edited.Text)
               );
        }

        Task Default()
        {
            this.logger.LogWarning("Could not handle Comment message" +
                                    $" with routing key {args.RoutingKey} and body {message}");
            return Task.CompletedTask;
        }
    }

    public async Task<List<Comment>> GetCommentsForUser(string username, int limit = 20, int skip = 0)
    {
        IQueryable<Comment> query = this.db.Comments.AsQueryable()
            .Where(c => c.ProfileId == username)
            .OrderByDescending(c => c.CreatedAt);

        if (skip > 0) query = query.Skip(skip);
        if (limit > 0) query = query.Take(limit);

        return await query.ToListAsync();
    }
}
