using CommentService.Models;

namespace CommentService.Services.Interfaces
{
    public interface ICommentPublisher
    {
        public Task Publish(Comment input);
    }
}
