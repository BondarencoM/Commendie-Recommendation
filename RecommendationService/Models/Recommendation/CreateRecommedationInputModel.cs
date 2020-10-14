using RecommendationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Recommendations
{
    public class CreateRecommedationInputModel
    {
        public long PersonId { get; set; }
        public long InterestId { get; set; }

        public Recommendation ToRecommendation() => new Recommendation() { PersonaId = PersonId, InterestId = InterestId, CreatedAt = DateTime.Now };
    }
}
