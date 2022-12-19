using RecommendationService.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecommendationService.Models.Personas;

public class Persona
{
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }
    public Uri ImageUri { get; set; }

    [JsonIgnore]
    public ICollection<Recommendation> Recommendations { get; set; }
    public string Description { get; set; }

    [Required]
    public string WikiId { get; set; }

    [Required]
    public string AddedBy { get; set; }

    public Uri WikipediaUri { get; set; }
}
