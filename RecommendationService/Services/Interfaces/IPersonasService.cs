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
        public Task<List<DiscoverPersonViewModel>> GetSuggestedForDiscovery(ushort limit);

        public Task<Persona> GetOrCreate(CreatePersonaInputModel input);
    }
}
