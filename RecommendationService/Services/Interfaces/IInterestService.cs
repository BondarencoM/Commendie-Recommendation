using RecommendationService.Models.Interests;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IInterestService : IUserCleanseable, IRepository<Interest, CreateInterestInputModel, CreateInterestInputModel>
{
    public Task<Interest> GetOrCreate(CreateInterestInputModel input);

}
