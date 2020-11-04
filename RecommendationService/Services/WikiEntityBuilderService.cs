using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Wikibase;

namespace RecommendationService.Services
{
    public class WikiEntityBuilderService : IWikiEntityBuilderService
    {
        readonly WikiSite wiki;

        public WikiEntityBuilderService(WikiSite wiki)
        {
            this.wiki = wiki;
        }

        public async Task<Entity> GetEntity(string wikiId, EntityQueryOptions options = EntityQueryOptions.FetchAllProperties)
        {
            await wiki.Initialization;
            var entity = new Entity(wiki, wikiId);
            await entity.RefreshAsync(options);

            return entity;
        }
    }
}
