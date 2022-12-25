using CommentService.Models;

namespace CommentService.Services.Interfaces;

public interface ICommentService : IHasDownloadableUserData
{
    public Task<Comment> Create(CreateCommentInputModel input);
    public Task Delete(int id);
    public Task Edit(EditCommentInputModel newComment);
}
