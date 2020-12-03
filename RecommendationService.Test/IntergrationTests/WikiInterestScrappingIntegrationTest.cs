using FluentAssertions;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Interests;
using RecommendationService.Models.Personas;
using RecommendationService.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiClientLibrary.Client;
using WikiClientLibrary.Sites;
using Xunit;
using Xunit.Abstractions;

namespace RecommendationService.Test.IntergrationTests
{
    public class WikiInterestScrappingIntegrationTest
    {
        readonly WikiInterestScrappingService Service;
        readonly WikiSite WikiSite;
        readonly ITestOutputHelper Output;

        string MobyDickId => "Q174596";
        string NinjaTurtlesId => "Q249056";
        string SoftwareEngineering => "Q80993";

        public WikiInterestScrappingIntegrationTest(ITestOutputHelper output)
        {
            Output = output;

            var wikiClient = new WikiClient { ClientUserAgent = "WCLQuickStart/1.0 bondarencom" };
            WikiSite = new WikiSite(wikiClient, "https://www.wikidata.org/w/api.php");
            Service = new WikiInterestScrappingService(WikiSite);
        }

        [Fact]
        public async Task ScrapePersonaDetails_FindsPersonById()
        {
            Interest actual = await Service.ScrapeInterestDetails(MobyDickId);

            var expected = new Interest()
            {
                Name = "Moby-Dick",
                WikiId = MobyDickId,
                Id = default,
                AddedBy = null,
                Type = InterestType.Book,
                
            };

            actual.Should().BeEquivalentTo(expected,
                options => options.Excluding(p => p.Description).Excluding(p=>p.ImageUri),
                "because we scrapped info for Moby-Dick");

            actual.Description.Should().NotBeNullOrEmpty("because we don't know what the description will be but there must be some");
            actual.ImageUri.ToString().Should().MatchRegex(@"https://commons\.wikimedia\.org/wiki/Special:FilePath/.+\..+$", "because there must be a link to an image");
        }

        [Fact]
        public async Task ScrapePersonaDetails_FindsPersonWithMultipleInstanceOfValues()
        {
            Interest actual = await Service.ScrapeInterestDetails(NinjaTurtlesId);

            var expected = new Interest()
            {
                Name = "Teenage Mutant Ninja Turtles",
                WikiId = NinjaTurtlesId,
                Id = default,
                AddedBy = null,
                Type = InterestType.Movie,
            };

            actual.Should().BeEquivalentTo(expected,
                options => options.Excluding(p => p.Description).Excluding(p => p.ImageUri),
                "because we scrapped info for Teenage Mutant Ninja Turtles");
        }

        [Fact]
        public async Task ScrapePersonaDetails_InexistantPersona()
        {
            await Service.Awaiting(s => s.ScrapeInterestDetails("Q999999999999999"))
                .Should().ThrowAsync<EntityNotFoundException>($"because entity with id Q999999999999999 doesn't exist");
        }

        [Fact]
        public async Task ScrapePersonaDetails_ThrowsIfItsNotAValidInterest()
        {
            await Service.Awaiting(s => s.ScrapeInterestDetails(SoftwareEngineering))
                .Should().ThrowAsync<AddedEntityIsNotAnInterestException>($"because entity with id {SoftwareEngineering} is not a Book, Movie, Game or other supported interest type");
        }
    }
}
