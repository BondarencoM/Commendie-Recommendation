using System.ComponentModel.DataAnnotations.Schema;

namespace CommentService.Models
{
    public class Comment
    {
        public Comment() { }
        public Comment(CreateCommentInputModel input)
        {
            this.Text = input.Text;
            this.Domain = input.Domain;
            this.EntityId = input.EntityId;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Domain { get; set; } = string.Empty;

        public string EntityId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
