using RecommendationService.Models;
using RecommendationService.Models.Interests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces
{
    public interface IInterestService: IRepository<Interest, CreateInterestInputModel, CreateInterestInputModel>
    {
        public Task<Interest> GetOrCreate(CreateInterestInputModel input);

    }
}
