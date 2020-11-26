using RecommendationService.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecommendationService.Models.Personas
{
    public class Persona
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string ImageUri { get; set; }

        [JsonIgnore]
        public List<Recommendation> Recommendations { get; set; }
        public string Description { get; set; }

        [Required]
        public string WikiId { get; set; }

        [Required]
        public string AddedBy { get; set; }

        public string WikipediaUri { get; set; }
    }
}
