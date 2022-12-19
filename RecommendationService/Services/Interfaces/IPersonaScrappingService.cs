using RecommendationService.Models.Personas;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IPersonaScrappingService
{
    Task<Persona> ScrapePersonaDetails(string identifier);
}
