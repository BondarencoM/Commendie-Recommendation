using CommentService.Exceptions;
using CommentService.Extensions;
using CommentService.Models;
using CommentService.Models.Messages;
using CommentService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        var fromDb = db.Comments.Add(comment).Entity;
        await db.SaveChangesAsync();

        await publisher.Created(new CreateCommentMessage(fromDb, input.Text ?? ""));

        return fromDb;
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

        if (!CanEdit(fromDb)) 
            throw new OperationNotPermittedException(this.principal, "delete", $"comment id={commentId.Id}");
        

        db.Remove(fromDb);

        await db.SaveChangesAsync();
        await publisher.Deleted(commentId);
    }

    public async Task Edit(EditCommentInputModel newComment)
    {
        var fromDb = await db.Comments.FindAsync(newComment.Id);

        if (fromDb is null) throw new CommentNotFoundException();

        if (!CanEdit(fromDb))
            throw new OperationNotPermittedException(this.principal, "delete", $"comment id={newComment.Id}");

        await publisher.Edited(new EditCommentMessage(
            newComment.Id,
            newComment.Text ?? "",
            fromDb.Domain));
    }

    public async Task<PersonalDataModel> GetDownloadableUserData(string username)
    {
        var comments = await db.Comments.Where(c => c.Username == username).ToListAsync();
        return new PersonalDataModel
        {
            Name = "comments",
            JsonData = comments,
        };
    }

    private bool CanEdit(Comment comment) => this.principal.IsAdmin() || CommentBelongsToUser(comment);
    private bool CommentBelongsToUser(Comment comment) => comment.Username is not null && comment.Username == this.Username;
}
