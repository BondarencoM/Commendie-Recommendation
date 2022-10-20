using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RecommendationService.Models;
using RecommendationService.Models.Comments;
using RecommendationService.Models.Recommendations;
using RecommendationService.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecommendationService.Services
{
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
            switch (args.RoutingKey)
            {
                case "comments.recommendation.new":
                    return AddComment();
                default:
                    this.logger.LogError($"Could not handle Comment message" +
                        $" with routing key {args.RoutingKey} and body {message}");
                    return Task.CompletedTask;
            }

            async Task AddComment()
            {
                var newComment = JsonSerializer.Deserialize<CreateCommentInputModel>(message);
                var comment = new Comment(newComment);
                this.db.Comments.Add(comment);

                await this.db.SaveChangesAsync();
            }
        }

        public async Task<List<Comment>> GetCommentsForRecommendation(long id, int limit = 20, int skip = 0)
        {
            IQueryable<Comment> query = this.db.Comments.AsQueryable()
                .Where(c => c.RecommendationId == id)
                .OrderByDescending(c => c.CreatedAt);

            if (skip > 0) query = query.Skip(skip);
            if (limit > 0) query = query.Take(limit);

            return await query.ToListAsync();
        }
    }
}
