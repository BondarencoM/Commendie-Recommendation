using RecommendationService.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces
{
    public interface IRecommendationService : IRepository<Recommendation, CreateRecommedationInputModel, UpdateRecommendationInputModel>
    {
    }
}
