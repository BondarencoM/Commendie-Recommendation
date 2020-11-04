using System.Threading.Tasks;
using WikiClientLibrary.Wikibase;

namespace RecommendationService.Services.Interfaces
{
    public interface IWikiEntityBuilderService
    {
        public Task<Entity> GetEntity(string wikiId, EntityQueryOptions options = EntityQueryOptions.FetchAllProperties);
    }
}