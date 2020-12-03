using RecommendationService.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Personas
{
    public class PersonaWithInterestsViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public Uri ImageUri { get; set; }

        public ICollection<RecommendationWithInterestViewModel> Recommendations { get; set; }
        public string Description { get; internal set; }

        public string WikiId { get; internal set; }

        public string AddedBy { get; set; }

        public Uri WikipediaUri { get; set; }


        public PersonaWithInterestsViewModel(Persona p)
        {
            Id = p.Id;
            Name = p.Name;
            ImageUri = p.ImageUri;
            Recommendations = p.Recommendations.Select(r => new RecommendationWithInterestViewModel(r)).ToList();
            Description = p.Description;
            WikiId = p.WikiId;
            AddedBy = p.AddedBy;
            WikipediaUri = p.WikipediaUri;
        }
    }
}
