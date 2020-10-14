using RecommendationService.Models.Personas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces
{
    public interface IPersonaScrappingService
    {
        Task<Persona> ScrapePersonaDetails(string url);
    }
}
