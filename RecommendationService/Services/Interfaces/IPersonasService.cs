using Microsoft.AspNetCore.Mvc;
using RecommendationService.Models;
using RecommendationService.Models.Personas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces
{
    public interface IPersonasService: IRepository<Persona, CreatePersonaInputModel, UpdatePersonaInputModel>
    {
        public Task<List<PersonaWithInterestsViewModel>> GetSuggestedForDiscovery(ushort limit);

        Task<PersonaWithInterestsViewModel> GetPersonaWithRecommendations(long id);
        Task<List<PersonaWithInterestsViewModel>> GetPersonasSearch(string search);
    }
}
