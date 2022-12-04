namespace CommentService.Models.Messages;

public class EditCommentMessage
{
    public EditCommentMessage(int id, string text, string domain)
    {
        Id = id;
        Text = text;
        Domain = domain;
    }

    public int Id { get; set; }

    public string Text { get; set; }

    public string Domain { get; set; }
}
