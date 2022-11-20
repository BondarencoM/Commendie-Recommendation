using CommentService.Exceptions;
using CommentService.Models;
using CommentService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CommentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentService commentService;

    public CommentsController(ILogger<CommentsController> logger, ICommentService commentService)
    {
        _logger = logger;
        this.commentService = commentService;
    }

    [HttpPost]
    [EnableRateLimiting("AddComment")]
    public async Task<ActionResult<Comment>> AddComment(CreateCommentInputModel input)
    {
        Comment fromDb = await this.commentService.Create(input);
        // TODO: replace with 201 Created
        return this.Created(this.Request.GetEncodedUrl() + "/" + fromDb.Id.ToString(), fromDb);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<Comment>> Delete(int id)
    {
        try
        {
            await this.commentService.Delete(id);
        }
        catch (CommentNotFoundException)
        {
            return this.NotFound();
        }
        catch (OperationNotPermittedException e)
        {
            this._logger.LogWarning(e, "");
            return this.Forbid();
        }

        return this.NoContent();
    }
}