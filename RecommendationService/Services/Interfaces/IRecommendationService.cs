using RecommendationService.Models.Recommendations;

namespace RecommendationService.Services.Interfaces;

public interface IRecommendationService : IUserCleanseable, IRepository<Recommendation, CreateRecommedationInputModel, UpdateRecommendationInputModel>
{
}
