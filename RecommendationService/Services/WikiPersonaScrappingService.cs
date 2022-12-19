using RecommendationService.Extensions;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Personas;
using RecommendationService.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using WikiClientLibrary;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Wikibase;

namespace RecommendationService.Services;

public class WikiPersonaScrappingService : IPersonaScrappingService
{
    private readonly WikiSite wiki;

    public WikiPersonaScrappingService(WikiSite wiki)
    {
        this.wiki = wiki;
    }

    public async Task<Persona> ScrapePersonaDetails(string identifier)
    {
        await wiki.Initialization;

        var entity = new Entity(wiki, identifier);
        try
        {
            await entity.RefreshAsync(EntityQueryOptions.FetchAllProperties);
        }
        catch(OperationFailedException e) when (e.Message.Contains("no-such-entity", StringComparison.OrdinalIgnoreCase))
        {
             throw new EntityNotFoundException(e.Message);
        }

        if (entity.IsHuman() == false) 
            throw new AddedEntityIsNotHumanException(entity);

        var model = new Persona()
        {
            Name = entity.Labels["en"],
            Description = entity.Descriptions["en"],
            WikiId = entity.Id,
            ImageUri = entity.ImageUris().FirstOrDefault(),
            WikipediaUri = entity.WikipediaLink(),
        };

        return model;
    }
}
