using RecommendationService.Models.Recommendations;

namespace RecommendationService.Services.Interfaces;

public interface IRecommendationService : IHasUserData, IRepository<Recommendation, CreateRecommedationInputModel, UpdateRecommendationInputModel>
{
}
