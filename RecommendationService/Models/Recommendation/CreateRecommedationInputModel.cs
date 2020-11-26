using RecommendationService.Models;
using RecommendationService.Models.Interests;
using RecommendationService.Models.Personas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Recommendations
{
    public class CreateRecommedationInputModel
    {
        public long PersonaId {get; set;}
        public long InterestId {get; set;}
        public string Context { get; set; }
    }
}
