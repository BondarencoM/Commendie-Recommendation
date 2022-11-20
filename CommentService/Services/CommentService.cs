using CommentService.Exceptions;
using CommentService.Extensions;
using CommentService.Models;
using CommentService.Services.Interfaces;
using System.Security.Principal;

namespace CommentService.Services;

public class CommentService : ICommentService
{
    private readonly DatabaseContext db;
    private readonly ICommentPublisher publisher;
    private readonly IPrincipal principal;
    private readonly ILogger<CommentService> logger;

    public CommentService(
        DatabaseContext db,
        ICommentPublisher publisher,
        IPrincipal principal,
        ILogger<CommentService> logger)
    {
        this.db = db;
        this.publisher = publisher;
        this.principal = principal;
        this.logger = logger;
    }

    private string? Username => this.principal?.Identity?.Name;


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
        await publisher.Created(comment);

        return commentFromDb;
    }

    public async Task Delete(int id)
    {
        var fromDb = await db.Comments.FindAsync(id);
        var commentId = new DeleteCommentMessage { 
            Id = id,
            Domain = fromDb?.Domain,
        };

        if (fromDb is null)
        {
            // Publish deleition anyway in case a client
            // was offline last time we deleted it
            await publisher.Deleted(commentId);
            throw new CommentNotFoundException();
        }

        if (!CanDelete()) throw new OperationNotPermittedException(this.principal, "delete", $"comment id={commentId.Id}");
        

        db.Remove(fromDb);

        await db.SaveChangesAsync();
        await publisher.Deleted(commentId);

        bool CanDelete() => this.principal.IsAdmin() || CommentBelongsToUser();
        bool CommentBelongsToUser() => fromDb.Username is not null && fromDb.Username == this.Username;
    }
}
