using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecommendationService.Models;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Interests;
using RecommendationService.Models.Personas;
using RecommendationService.Models.Recommendations;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace RecommendationService.Services
{
    public class RecommednationService : IRecommendationService
    {

        private readonly DatabaseContext db;
        private readonly IServiceProvider _services;

        private IPersonasService PersonasService => _services.GetService<IPersonasService>();
        private IInterestService InterestsService => _services.GetService<IInterestService>();
        private readonly IPrincipal _principal;

        private string PrincipalUsername => _principal.Identity.Name;

        public RecommednationService(
            DatabaseContext db, 
            IServiceProvider services,
            IPrincipal principal)
        {
            this.db = db;
            _services = services;
            _principal = principal;
        }

        public async Task<Recommendation> Add(CreateRecommedationInputModel input)
        {

            var alreadyExists = await db.Recommendation.AsQueryable()
                                        .Where(r => r.Interest.WikiId == input.Interest.WikiId)
                                        .Where(r => r.Persona.WikiId == input.Persona.WikiId)
                                        .SingleOrDefaultAsync();
            if(alreadyExists != null)
            {
                throw new EntityAlreadyExists<Recommendation>(alreadyExists);
            }
            //TODO: For now is synchronous, find a way to run this asynchronously
            Task<Persona> getPersona = PersonasService.GetOrCreate(input.Persona);
            Persona persona = await getPersona;
            Task<Interest> getInterest = InterestsService.GetOrCreate(input.Interest);
            Interest interest = await getInterest;

            var recommendation = new Recommendation()
            {
                Interest = interest,
                Persona = persona,
                Context = input.Context,
                AddedBy = PrincipalUsername,
                CreatedAt = DateTime.Now,
            };

            var fromDb = db.Recommendation.Add(recommendation);

            await db.SaveChangesAsync();

            return fromDb.Entity;
        }

        public async Task<List<Recommendation>> All()
        {
            return await db.Recommendation.AsQueryable().ToListAsync();
        }

        public async Task<Recommendation> Find(long id)
        {
            return await db.Recommendation
                                .Include(r => r.Interest)
                                .Include(r => r.Persona)
                                .Where(r => r.Id == id)
                                .SingleOrDefaultAsync();
        }

        public Task Update(long id, CreateRecommedationInputModel persona)
        {
            throw new NotImplementedException();
        }
    }
}
