namespace CommentService.Models.Messages;

public class CreateCommentMessage
{
    public CreateCommentMessage(Comment fromDb, string text)
    {
        this.Id = fromDb.Id;
        this.Domain = fromDb.Domain;
        this.EntityId = fromDb.EntityId;
        this.Username = fromDb.Username;
        this.CreatedAt = fromDb.CreatedAt;
        this.Text = text;
    }

    public int Id { get; set; }

    public string Text { get; set; }

    public string Domain { get; set; }

    public string EntityId { get; set; }

    public string? Username { get; set; }

    public DateTime CreatedAt { get; set; }
}
