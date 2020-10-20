using Microsoft.EntityFrameworkCore;
using RecommendationService.Models;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Personas;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;

namespace RecommendationService.Services
{
    public class PersonasService : IPersonasService
    {
        private readonly DatabaseContext db;
        private readonly IPersonaScrappingService scrappingService;
        private readonly IPrincipal _principal;

        private string PrincipalUsername => _principal.Identity.Name;

        public PersonasService(DatabaseContext db, IPersonaScrappingService scrappingService, IPrincipal principal)
        {
            this.db = db;
            this.scrappingService = scrappingService;
            _principal = principal;
        }

        public async Task<List<Persona>> All()
        {
            return await db.Personas.AsQueryable().ToListAsync();
        }
        public async Task<Persona> Find(long id)
        {
            return await db.Personas.FindAsync(id);
        }

        public async Task<Persona> Add(CreatePersonaInputModel input)
        {

            Persona currentVersion = await db.Personas.AsQueryable()
                                                .Where(p => p.WikiId == input.WikiId)
                                                .SingleOrDefaultAsync();
            if (currentVersion != null)
                throw new EntityAlreadyExists<Persona>(currentVersion);

            Persona model = await scrappingService.ScrapePersonaDetails(input.WikiId);
            model.AddedBy = PrincipalUsername;
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

        public async Task<Persona> GetOrCreate(CreatePersonaInputModel input)
        {
            var persona = await db.Personas.AsQueryable()
                                    .Where(p => p.WikiId == input.WikiId)
                                    .SingleOrDefaultAsync();

            return persona ?? await Add(input);
        }
    }
}
