using CommentService.Models;
using CommentService.Services.Interfaces;

namespace CommentService.Services
{
    public class CommentService : ICommentService
    {
        private readonly DatabaseContext db;

        public CommentService(DatabaseContext db)
        {
            this.db = db;
        }

        public async Task<Comment> Create(CreateCommentInputModel input)
        {
            var comment = new Comment(input);
            var fromDb = db.Comments.Add(comment);
            await db.SaveChangesAsync();

            return fromDb.Entity;
        }
    }
}
