namespace CommentService.Models
{
    public class CreateCommentInputModel
    {
        public string Text { get; set; }

        public string Domain { get; set; }

        public string EntityId { get; set; }
    }
}