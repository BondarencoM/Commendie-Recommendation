using System;

namespace RecommendationService.Models.Comments;

public class CreateCommentInputModel
{
    public long Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Domain { get; set; } = string.Empty;

    public string EntityId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
