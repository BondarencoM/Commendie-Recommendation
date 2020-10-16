using Microsoft.EntityFrameworkCore;
using RecommendationService.Models;
using RecommendationService.Models.Personas;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecommendationService.Services
{
    public class PersonasService : IPersonasService
    {
        private readonly DatabaseContext db;
        private readonly IPersonaScrappingService scrappingService;

        public PersonasService(DatabaseContext db, IPersonaScrappingService scrappingService)
        {
            this.db = db;
            this.scrappingService = scrappingService;
        }

        public async Task<List<Persona>> All()
        {
            return await db.Personas.AsQueryable().ToListAsync();
        }
        public async Task<Persona> Find(long id)
        {
            return await db.Personas.FindAsync(id);
        }

        public async Task<Persona> Add(CreatePersonaInputModel persona)
        {
            Persona model = await scrappingService.ScrapePersonaDetails(persona.WikiId);

            var fromDb = db.Add(model);
            await db.SaveChangesAsync();

            return fromDb.Entity;
        }

        public async Task<List<DiscoverPersonViewModel>> GetSuggestedForDiscovery(ushort limit)
        {
            return await db.Personas.AsQueryable()
            .Take(limit)
            .Include(p => p.Recommendations)
                .ThenInclude(r => r.Interest)
            .Select(p => new DiscoverPersonViewModel(p))
            .ToListAsync();
        }

        public Task Update(long id, UpdatePersonaInputModel persona)
        {
            throw new NotImplementedException();
        }
    }
}
