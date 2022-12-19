using RecommendationService.Models.Interests;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IInterestScrappingService
{
    Task<Interest> ScrapeInterestDetails(string identifier);
}
