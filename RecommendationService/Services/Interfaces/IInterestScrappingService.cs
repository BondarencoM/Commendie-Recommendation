using RecommendationService.Models.Interests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces
{
    public interface IInterestScrappingService
    {
        Task<Interest> ScrapeInterestDetails(string identifier);
    }
}
