using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using RecommendationService.Models;
using RecommendationService.Services;
using RecommendationService.Services.Interfaces;
using RecommendationService.Test.TestHelpers;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using WikiClientLibrary.Client;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Wikibase;
using Xunit;
using Xunit.Abstractions;

namespace RecommendationService.Test.Services
{
    public class WikiPersonaScrappingServiceTest
    {
        readonly WikiPersonaScrappingService Service;
        readonly DatabaseContext db;
        readonly ITestOutputHelper Output;

        readonly Mock<IWikiEntityBuilderService> MockWiki;

        readonly string SampleWikiId = "Q18032000";

        public WikiPersonaScrappingServiceTest(ITestOutputHelper output)
        {
            Output = output;
            db = ContextBuilder.GetUniqueInMemory().Result;
            MockWiki = new Mock<IWikiEntityBuilderService>();
            Service = new WikiPersonaScrappingService(MockWiki.Object);
        }

        [Fact]
        public async Task ItIs()
        {
            MockWiki.Setup(
                wiki => wiki.GetEntity(SampleWikiId, It.IsAny<EntityQueryOptions>())
            ).ReturnsAsync(new Entity(new WikiSite(new Mock<IWikiClient>().Object, "https://www.wikidata.org/w/api.php"), SampleWikiId) { 
                
            });

            var persona = await Service.ScrapePersonaDetails(SampleWikiId);
        }
    }
}
