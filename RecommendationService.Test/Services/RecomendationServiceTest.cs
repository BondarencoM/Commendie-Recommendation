using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecommendationService.Models;
using RecommendationService.Models.Personas;
using RecommendationService.Services;
using RecommendationService.Services.Interfaces;
using RecommendationService.Test.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RecommendationService.Test
{
    public class RecomendationServiceTest: IDisposable
    {
        private readonly PersonasService service;
        private readonly DatabaseContext db;
        private readonly ITestOutputHelper Output;

        private readonly List<Persona> samplePersonas;

        public  RecomendationServiceTest(ITestOutputHelper output)
        {
            Output = output;

            db = ContextBuilder.GetUniqueInMemory().Result;

            samplePersonas = new List<Persona>()
            {
                new Persona { Id = 1, WikiId = "Q5006102",    Name = "CGP Grey", },
                new Persona { Id = 2, WikiId = "Q4955182",    Name = "Brady Haran" },
                new Persona { Id = 3, WikiId = "Q95137087",   Name = "Jon Skeet" },
                new Persona { Id = 4, WikiId = "Q630446",     Name = "John Green" },
                new Persona { Id = 5, WikiId = "Q19837",      Name = "Steve Jobs" },
                new Persona { Id = 6, WikiId = "Q5284",       Name = "Bill Gates" },
            }.Select(p => {
                p.AddedBy = "@xtronik";
                return p;
            }).ToList();

            var mockScrapping = new Mock<IPersonaScrappingService>();
            var mockPrincipal = new Mock<IPrincipal>();

            db.Personas.AddRange(samplePersonas);
            db.SaveChanges();

            service = new PersonasService(db, mockScrapping.Object, mockPrincipal.Object);
        }

        public void Dispose() => db.Dispose();

        [Fact]
        public async Task All_ReturnsAllPersona()
        {
            var actual = await service.All();
            actual.Should().BeEquivalentTo(samplePersonas, because: "that's what we have in the database");
        }

        [Fact]
        public async Task All_ReturnsEmptyWhenThereIsNoData()
        {
            db.RemoveRange(db.Personas);
            await db.SaveChangesAsync();
            (await service.All())
                .Should().BeEmpty(because: "there is nothing in the database");
        }

        [Fact]
        public async Task Find_ReturnsAPersona()
        {
            var actual = await service.Find(1);
            actual.Should().BeEquivalentTo(samplePersonas.Where(p => p.Id == 1).First(), because: "we're getting the person with id 1");
        }

        [Fact]
        public async Task Find_ReturnsNullWhenNotFound()
        {
            var actual = await service.Find(45665445665444L);
            actual.Should().BeNull(because: "we're there shouldn't be any user with that id");
        }

        
    }
}
