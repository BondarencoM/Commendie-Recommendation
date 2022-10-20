using RecommendationService.Models.Interests;
using RecommendationService.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RecommendationService.Models.Comments
{
    public class Comment
    {
        public Comment()
        {
        }

        public Comment(CreateCommentInputModel newComment)
        {
            this.Id = newComment.Id;
            this.Text = newComment.Text;
            this.Username = newComment.Username;
            this.CreatedAt = newComment.CreatedAt;

            var id = long.Parse(newComment.EntityId, CultureInfo.InvariantCulture);

            if (newComment.Domain is "recommendation") this.RecommendationId = id;
            else if (newComment.Domain is "interest") this.InterestId = id;
            else throw new InvalidOperationException("Could not create commeent with domain =" + newComment.Domain);
        }

        public long Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public long? RecommendationId { get; set; }

        public long? InterestId { get; set; }

        public bool IsDeleted { get; set; }

        public Recommendation Recommendation { get; set; }

        public Interest Interest { get; set; }
    }
}
