namespace CommentRecommendationService.Models.User;

public class DownloadablePersonalDataMessage
{
    public DownloadablePersonalDataMessage(object data, string username)
    {
        this.Username = username;
        this.JsonData = data;
    }

    public string Username { get; set; }

    public string Domain { get; set; } = "recommendation";

    public object JsonData { get; set; }
}
