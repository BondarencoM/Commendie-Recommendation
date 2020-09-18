using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecommendationService.Models
{
    public class Persona
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageURI { get; set; }

        [JsonIgnore]
        public List<Recommendation> Recommendations { get; set; }

    }
}
