using System.ComponentModel.DataAnnotations;

namespace RecommendationService.Models.Personas;

public class CreatePersonaInputModel
{
    [Required]
    public string WikiId { get; set; }
}
