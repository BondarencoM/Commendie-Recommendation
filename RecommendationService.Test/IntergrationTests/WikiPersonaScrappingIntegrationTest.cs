using FluentAssertions;
using RecommendationService.Models.Exceptions;
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
    public class WikiPersonaScrappingIntegrationTest
    {
        readonly WikiPersonaScrappingService Service;
        readonly WikiSite WikiSite;
        readonly ITestOutputHelper Output;


        string DavidBowieId => "Q5383";
        string LaikaDogId => "Q53662";
        string GeorgakisId => "Q87987698";

        public WikiPersonaScrappingIntegrationTest(ITestOutputHelper output)
        {
            Output = output;

            var wikiClient = new WikiClient { ClientUserAgent = "WCLQuickStart/1.0 bondarencom" };
            WikiSite = new WikiSite(wikiClient, "https://www.wikidata.org/w/api.php");
            Service = new WikiPersonaScrappingService(WikiSite);
        }

        [Fact]
        public async Task ScrapePersonaDetails_FindsPersonById()
        {
            Persona actual = await Service.ScrapePersonaDetails(DavidBowieId);

            var expected = new Persona()
            {
                Name = "David Bowie",
                AddedBy = null,
                Id = default,
                Recommendations = null,
                WikiId = DavidBowieId,
                WikipediaUri = new Uri("https://en.wikipedia.org/wiki/David_Bowie"),
            };

            actual.Should().BeEquivalentTo(expected,
                options => options.Excluding(p => p.Description).Excluding(p=>p.ImageUri),
                "because we scrapped info for David Bowie");

            actual.Description.Should().NotBeNullOrEmpty("because we don't know what the description will be but there must be some");
            actual.ImageUri.ToString().Should().MatchRegex(@"https://commons\.wikimedia\.org/wiki/Special:FilePath/.+\..+$", "because there must be a link to an image");
        }

        [Fact]
        public async Task ScrapePersonaDetails_FindsPersonWithMultipleInstanceOfValues()
        {
            Persona actual = await Service.ScrapePersonaDetails(GeorgakisId);

            var expected = new Persona()
            {
                Name = "Georgakis Mourdoukoutas",
                WikiId = GeorgakisId,
                AddedBy = null,
                Id = default,
                Recommendations = null,
            };

            actual.Should().BeEquivalentTo(expected,
                options => options.Excluding(p => p.Description).Excluding(p => p.ImageUri).Excluding(p => p.WikipediaUri),
                "because we scrapped info for Georgakis Mourdoukoutas");
        }

        [Fact]
        public async Task ScrapePersonaDetails_InexistantPersona()
        {
            await Service.Awaiting(s => s.ScrapePersonaDetails("Q999999999999999"))
                .Should().ThrowAsync<EntityNotFoundException>($"because entity with id Q999999999999999 shouldn't exist");
        }

        [Fact]
        public async Task ScrapePersonaDetails_ThrowsIfItsNotAHuman()
        {
            await Service.Awaiting(s => s.ScrapePersonaDetails(LaikaDogId))
                .Should().ThrowAsync<AddedEntityIsNotHumanException>($"because entity with id {LaikaDogId} is a dog and not a human");
        }
    }
}
