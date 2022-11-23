using RecommendationService.Models.Interests;
using System;

namespace RecommendationService.Models.Recommendations
{
    public class RecommendationWithInterestViewModel
    {
        public RecommendationWithInterestViewModel(Recommendation r)
        {
            Id = r.Id;
            CreatedAt = r.CreatedAt;
            Context = r.Context;
            Interest = r.Interest;
            AddedBy = r.AddedBy;
            IsConfirmed = r.IsConfirmed;
        }

        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Context { get; set; }

        public long PersonaId { get; set; }
        public long InterestId { get; set; }

        public Interest Interest { get; set; }

        public string AddedBy { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
