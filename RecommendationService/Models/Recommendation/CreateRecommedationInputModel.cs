namespace RecommendationService.Models.Recommendations;

public class CreateRecommedationInputModel
{
    public long PersonaId {get; set;}
    public long InterestId {get; set;}
    public string Context { get; set; }
}
