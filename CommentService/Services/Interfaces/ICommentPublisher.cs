using CommentService.Models;
using CommentService.Models.Messages;

namespace CommentService.Services.Interfaces;

public interface ICommentPublisher
{
    public Task Created(CreateCommentMessage input);

    public Task Deleted(DeleteCommentMessage input);

    public Task Edited(EditCommentMessage newComment);
}
