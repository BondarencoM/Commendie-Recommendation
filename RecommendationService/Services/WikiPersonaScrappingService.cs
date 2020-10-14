using RecommendationService.Extensions;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Personas;
using RecommendationService.Models.Wikibase;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WikiClientLibrary.Client;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Wikibase;

namespace RecommendationService.Services
{
    public class WikiPersonaScrappingService : IPersonaScrappingService, IDisposable
    {
        private readonly HttpClient http;
        private readonly WikiClient client;
        private readonly WikiSite wikiSite;

        public WikiPersonaScrappingService(HttpClient http)
        {
            this.http = http;
            client = new WikiClient
            {
                ClientUserAgent = "WCLQuickStart/1.0 bondarencom"
            };
            wikiSite = new WikiSite(client, "https://www.wikidata.org/w/api.php");
        }

        public void Dispose()
        {
            ((IDisposable)client).Dispose();
        }

        public async Task<Persona> ScrapePersonaDetails(string wikiId)
        {
            await wikiSite.Initialization;

            var entity = new Entity(wikiSite, wikiId);
            await entity.RefreshAsync(EntityQueryOptions.FetchAllProperties);

            if (entity.IsHuman() == false) 
                throw new AddedEntityIsNotHuman(entity);

            var model = new Persona()
            {
                Name = entity.Labels["en"],
                Description = entity.Descriptions["en"],
                WikiId = entity.Id,
            };

            return model;
        }
    }
}
