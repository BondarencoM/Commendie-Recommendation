using RecommendationService.Models.Recommendations;

namespace RecommendationService.Services.Interfaces
{
    public interface IRecommendationService : IRepository<Recommendation, CreateRecommedationInputModel, UpdateRecommendationInputModel>
    {
    }
}
