using CommentService.Models;

namespace CommentService.Services.Interfaces;

public interface ICommentPublisher
{
    public Task Created(Comment input);

    public Task Deleted(DeleteCommentMessage input);
}
