using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendationService.Models;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Interests;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using WikiClientLibrary.Sites;

namespace RecommendationService.Services
{
    public class InterestService : IInterestService
    {
        private readonly DatabaseContext db;
        private readonly IInterestScrappingService _scrappingService;
        private readonly IPrincipal _principal;

        private string PrincipalUsername => _principal.Identity.Name;

        public InterestService(DatabaseContext db, IInterestScrappingService scrappingService, IPrincipal principal)
        {
            this.db = db;
            _scrappingService = scrappingService;
            _principal = principal;
        }

        public async Task<List<Interest>> All()
        {
            return await db.Interests.AsQueryable().ToListAsync();
        }

        public async Task<Interest> Find(long id)
        {
            return await db.Interests.FindAsync(id);
        }

        public async Task<Interest> Add(CreateInterestInputModel input)
        {
            Interest currentVersion = await db.Interests.AsQueryable()
                                                .Where(e => e.WikiId == input.WikiId)
                                                .SingleOrDefaultAsync();

            if (currentVersion != null)
                throw new EntityAlreadyExists<Interest>(currentVersion);

            Interest interest = await _scrappingService.ScrapeInterestDetails(input.WikiId);
            interest.AddedBy = PrincipalUsername;
            var fromDb = db.Interests.Add(interest);
            await db.SaveChangesAsync();

            return fromDb.Entity;

        }

        public Task Update(long id, CreateInterestInputModel persona)
        {
            throw new NotImplementedException();
        }

        public async Task<Interest> GetOrCreate(CreateInterestInputModel input)
        {
            var interest = await db.Interests.AsQueryable()
                        .Where(i => i.WikiId == input.WikiId)
                        .SingleOrDefaultAsync();

            return interest ?? await Add(input);
        }
    }
}
