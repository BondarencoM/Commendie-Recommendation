using ProfileService.Profiles;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace ProfileService.Comments;

public class Comment
{
    public Comment()
    {
    }

    public Comment(CreateCommentIM newComment)
    {
        this.Id = newComment.Id;
        this.Text = newComment.Text;
        this.ProfileId = newComment.Username;
        this.CreatedAt = newComment.CreatedAt;
        this.Domain = newComment.Domain;
        this.EntityId = newComment.EntityId;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    public string Text { get; set; } = string.Empty;

    [JsonIgnore]
    public string ProfileId { get; set; } = string.Empty;
    
    public string Username => ProfileId;

    public DateTime CreatedAt { get; set; }

    public string Domain { get; set; }

    public string EntityId { get; set; }

    public long? InterestId { get; set; }

    public bool IsDeleted { get; set; }

    [JsonIgnore]
    public Profile Profile { get; set; } = null!;
}
