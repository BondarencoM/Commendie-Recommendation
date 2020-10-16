using RecommendationService.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Personas
{
    public class DiscoverPersonViewModel
    {
        public Persona Persona { get; set; }

        public IEnumerable<Recommendation> Recommendations { get; set; }

        public DiscoverPersonViewModel(Persona persona)
        {
            Persona = persona;

            Recommendations = persona.Recommendations;
        }
    }
}
