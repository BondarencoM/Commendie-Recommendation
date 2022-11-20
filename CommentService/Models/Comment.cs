using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommentService.Models;

public class Comment
{
    public Comment(string domain, string entityId)
    {
        this.Domain = domain;
        this.EntityId = entityId;
    }

    public Comment(CreateCommentInputModel input)
    {
        this.Text = input.Text ?? throw new ArgumentException(nameof(input.Text) + " was null", nameof(input));
        this.Domain = input.Domain ?? throw new ArgumentException(nameof(input.Domain) + " was null", nameof(input));
        this.EntityId = input.EntityId ?? throw new ArgumentException(nameof(input.EntityId) + " was null", nameof(input));
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public string? Username { get; set; }

    public string Domain { get; set; }

    public string EntityId { get; set; }

    public DateTime CreatedAt { get; set; }
}
