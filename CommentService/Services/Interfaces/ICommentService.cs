using CommentService.Models;

namespace CommentService.Services.Interfaces;

public interface ICommentService
{
    public Task<Comment> Create(CreateCommentInputModel input);
    public Task Delete(int id);
}
