using RecommendationService.Models.Personas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IPersonasService : IHasUserData, IRepository<Persona, CreatePersonaInputModel, UpdatePersonaInputModel>
{
    public Task<List<PersonaWithInterestsViewModel>> GetSuggestedForDiscovery(ushort limit);

    Task<PersonaWithInterestsViewModel> GetPersonaWithRecommendations(long id);
    Task<List<PersonaWithInterestsViewModel>> GetPersonasSearch(string search);
}
