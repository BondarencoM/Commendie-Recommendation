namespace CommentService.Models.Messages;

public class DownloadablePersonalDataMessage
{
    public DownloadablePersonalDataMessage(object data, string username)
    {
        this.Username = username;
        this.JsonData = data;
    }

    public string Username { get; set; }

    public string Domain { get; set; } = "comment";

    public object JsonData { get; set; }
}
