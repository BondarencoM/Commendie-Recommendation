using FluentAssertions;
using Moq;
using RecommendationService.Models;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Interests;
using RecommendationService.Models.Personas;
using RecommendationService.Models.Recommendations;
using RecommendationService.Services;
using RecommendationService.Services.Interfaces;
using RecommendationService.Test.TestHelpers;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RecommendationService.Test
{
    public class PersonaServiceTest: IDisposable
    {
        private readonly PersonasService Service;
        private readonly DatabaseContext db;
        private readonly ITestOutputHelper Output;

        private readonly Mock<IPersonaScrappingService> mockScrapping;
        private readonly Mock<IPrincipal> mockPrincipal;

        private readonly string TestingUser = "TestingUser";

        public  PersonaServiceTest(ITestOutputHelper output)
        {
            Output = output;

            db = ContextBuilder.GetUniqueInMemory().Result;

            mockScrapping = new Mock<IPersonaScrappingService>();
            mockPrincipal = new Mock<IPrincipal>();

            mockPrincipal.Setup(p => p.Identity.Name).Returns(TestingUser);

            Service = new PersonasService(db, mockScrapping.Object, mockPrincipal.Object);
        }

        public void Dispose() => db.Dispose();

        [Fact]
        public async Task All_ReturnsAllPersona()
        {
            var actual = await Service.All();
            actual.Should().BeEquivalentTo(db.Personas, because: "that's what we have in the database");
        }

        [Fact]
        public async Task All_ReturnsEmptyWhenThereIsNoData()
        {
            db.RemoveRange(db.Personas);
            await db.SaveChangesAsync();
            (await Service.All())
                .Should().BeEmpty(because: "there is nothing in the database");
        }

        [Fact]
        public async Task Find_ReturnsAPersona()
        {
            var actual = await Service.Find(1);
            actual.Should().BeEquivalentTo(db.Personas.Find(1L), because: "we're getting the person with id 1");
        }

        [Fact]
        public async Task Find_ReturnsNullWhenNotFound()
        {
            var actual = await Service.Find(45665445665444L);
            actual.Should().BeNull("because there shouldn't be any user with that id");
        }

        [Fact]
        public async Task Add_AddsNewPersonaToDb()
        {
            var wikiId = "Q999999";
            var newPersona = new CreatePersonaInputModel() { WikiId = wikiId };

            var expectedPersona = new Persona()
            {
                WikiId = wikiId,
                Name = "John Doe",
                Description = "Not a real person",
                ImageUri = "SomeUrl.com",
                WikipediaUri ="Wiki.com",
                Recommendations = null,
                AddedBy = TestingUser,
            };

            mockScrapping.Setup(s => s.ScrapePersonaDetails(wikiId)).ReturnsAsync(new Persona()
            {
                WikiId = wikiId,
                Name = "John Doe",
                Description = "Not a real person",
                ImageUri = "SomeUrl.com",
                WikipediaUri = "Wiki.com",
                Recommendations = null,
            });

            var actual = await Service.Add(newPersona);

            actual.Should().BeEquivalentTo(expectedPersona, 
                options => options.Excluding(p => p.Id));

            db.Personas.AsQueryable().Where( p => p.WikiId == wikiId).FirstOrDefault()
                .Should().NotBeNull("because we want it to be saved in the database")
                .And.BeEquivalentTo(expectedPersona, options => options.Excluding(p => p.Id));
        }

        [Fact]
        public void Add_ThrowsIfPersonaExists()
        {

            var wikiId = db.Personas.First().WikiId;
            var newPersona = new CreatePersonaInputModel() { WikiId = wikiId };

            mockScrapping.Setup(s => s.ScrapePersonaDetails(wikiId)).ReturnsAsync(new Persona()
            {
                WikiId = wikiId,
                Name = "John Doe",
                Description = "Not a real person",
                ImageUri = "SomeUrl.com",
                WikipediaUri = "Wiki.com",
                Recommendations = null,
            });

            Service.Awaiting(s => s.Add(newPersona))
                .Should().Throw<EntityAlreadyExists<Persona>>("because a persona with that wikiId already exists");
        }

        [Fact]
        public async Task GetPersonaWithRecommendations_ReturnsAPersona()
        {
            var actual = await Service.GetPersonaWithRecommendations(1);
            var expected = new PersonaWithInterestsViewModel(db.Personas.Find(1L));

            actual.Should().BeEquivalentTo(expected, "because we're getting the person with id 1");
        }

        [Fact]
        public async Task GetPersonaWithRecommendations_ReturnsNullIfNoneFound()
        {
            var actual = await Service.GetPersonaWithRecommendations(999);
            actual.Should().BeNull("because there shouldn't be any persona with that Id");
        }

        [Fact]
        public async Task GetSuggestedForDiscovery_ReturnsPersonas()
        {
            ushort limit = 12;
            var actual = await Service.GetSuggestedForDiscovery(limit);
            actual.Should().HaveCount(Math.Min(limit, db.Personas.Count()));
        }

    }
}
