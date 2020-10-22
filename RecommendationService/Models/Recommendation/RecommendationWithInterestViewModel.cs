using RecommendationService.Models.Interests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        }

        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Context { get; set; }

        public long PersonaId { get; set; }
        public long InterestId { get; set; }

        public Interest Interest { get; set; }

        public string AddedBy { get; set; }
    }
}
