using RecommendationService.Models.User;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IHasUserData
{
    public Task CleanseUser(UserIdentifierIM user);

    public Task<PersonalDataModel> GetDownloadableUserData(UserIdentifierIM user);
}
