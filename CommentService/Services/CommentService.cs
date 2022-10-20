using CommentService.Models;
using CommentService.Services.Interfaces;
using System.Security.Principal;

namespace CommentService.Services
{
    public class CommentService : ICommentService
    {
        private readonly DatabaseContext db;
        private readonly ICommentPublisher publisher;
        private readonly IPrincipal? principal;

        public CommentService(DatabaseContext db, ICommentPublisher publisher, IPrincipal? principal)
        {
            this.db = db;
            this.publisher = publisher;
            this.principal = principal;
        }

        public async Task<Comment> Create(CreateCommentInputModel input)
        {
            var comment = new Comment(input)
            {
                Username = this.principal?.Identity?.Name,
                CreatedAt = DateTime.Now,
            };

            var fromDb = db.Comments.Add(comment);
            await db.SaveChangesAsync();

            var commentFromDb = fromDb.Entity;
            await publisher.Publish(comment);

            return commentFromDb;
        }
    }
}
