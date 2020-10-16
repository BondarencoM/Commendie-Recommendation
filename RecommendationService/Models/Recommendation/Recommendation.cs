using RecommendationService.Models.Interests;
using RecommendationService.Models.Personas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Recommendations
{
    public class Recommendation
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Context { get; set; }

        public long PersonaId { get; set; }
        public long InterestId { get; set; }

        public Persona Persona { get; set; }
        public Interest Interest { get; set; }
    }
}
