using CommentService.Models;
using CommentService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.Controllers
{
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
        public async Task<ActionResult<Comment>> AddComment(CreateCommentInputModel input)
        {
            Comment fromDb = await this.commentService.Create(input);

            // TODO: replace with 201 Created
            return this.Ok(fromDb);
        }
    }
}