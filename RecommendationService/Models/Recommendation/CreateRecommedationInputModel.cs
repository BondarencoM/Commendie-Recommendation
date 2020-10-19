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
        public CreatePersonaInputModel Persona {get; set;}
        public CreateInterestInputModel Interest {get; set;}
        public string Context { get; set; }
    }
}
