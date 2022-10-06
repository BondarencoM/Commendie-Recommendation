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
using System.Text.RegularExpressions;
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

        public async Task<Persona> Add(CreatePersonaInputModel persona)
        {
            Persona currentVersion = await db.Personas.AsQueryable()
                                                .Where(p => p.WikiId == persona.WikiId)
                                                .SingleOrDefaultAsync();
            if (currentVersion != null)
                throw new EntityAlreadyExistsException<Persona>(currentVersion);

            Persona model = await scrappingService.ScrapePersonaDetails(persona.WikiId);
            model.AddedBy = PrincipalUsername;
            var fromDb = db.Add(model);
            await db.SaveChangesAsync();

            return fromDb.Entity;
        }

        public async Task<List<PersonaWithInterestsViewModel>> GetSuggestedForDiscovery(ushort limit)
        {
            var personas = await db.Personas.AsQueryable()
            .Include(p => p.Recommendations)
                .ThenInclude(r => r.Interest)
            .OrderByDescending(p => p.Id)
            .Take(limit)
            .Select(p => new PersonaWithInterestsViewModel(p))
            .ToListAsync();

            personas.ForEach(p => p.Recommendations =  p.Recommendations.OrderByDescending(r => r.CreatedAt).ToList());

            return personas.OrderByDescending(p => p.Recommendations.FirstOrDefault()?.CreatedAt ?? new DateTime()).ToList();
        }

        public Task Update(long id, UpdatePersonaInputModel persona)
        {
            throw new NotImplementedException();
        }


        public async Task<PersonaWithInterestsViewModel> GetPersonaWithRecommendations(long id)
        {
            return await db.Personas.AsQueryable()
                               .Where(p => p.Id == id)
                               .Include(p => p.Recommendations)
                                   .ThenInclude(r => r.Interest)
                               .Select(p => new PersonaWithInterestsViewModel(p))
                               .SingleOrDefaultAsync();
        }

        public async Task<List<PersonaWithInterestsViewModel>> GetPersonasSearch(string search)
        {

            if (IsWikiId(search))
                return await db.Personas
                    .AsQueryable()
                    .Where(p => p.WikiId == search)
                    .Include(p => p.Recommendations)
                                   .ThenInclude(r => r.Interest)
                    .Select((p => new PersonaWithInterestsViewModel(p)))
                    .ToListAsync();
            else
                return await GetPersonasByNameSearch(search);


        }

        private static bool IsWikiId(string search) => Regex.IsMatch(search, "^Q[0-9]+$"); 

        private async Task<List<PersonaWithInterestsViewModel>> GetPersonasByNameSearch(string search)
        {
            var terms = search.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            // Pad the list with dummy strings to make sure we don't go out of the range
            // We will only look into teh first 5 words in the search
            while (terms.Count < 6)
                terms.Add("match_nothing");

            var query = db.Personas
                    .AsQueryable()
                    .Where( p =>
                        p.Name.ToLower().Contains(terms[0]) |
                        p.Name.ToLower().Contains(terms[1]) ||
                        p.Name.ToLower().Contains(terms[2]) ||
                        p.Name.ToLower().Contains(terms[3]) ||
                        p.Name.ToLower().Contains(terms[4]) 
                    )
                    .Take(40)
                    .Include(p => p.Recommendations)
                                   .ThenInclude(r => r.Interest)
                    .Select(p => new PersonaWithInterestsViewModel(p))
                    .ToListAsync();

            return await query;
        }
    }
}
