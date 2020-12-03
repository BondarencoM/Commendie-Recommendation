using RecommendationService.Models.Interests;
using RecommendationService.Models.Wikibase.WikibaseIdentifier;
using RecommendationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Wikibase;
using RecommendationService.Models.Wikibase;
using RecommendationService.Extensions;
using RecommendationService.Models.Exceptions;
using WikiClientLibrary;

namespace RecommendationService.Services
{
    public class WikiInterestScrappingService : IInterestScrappingService
    {
        private readonly WikiSite wiki;

        public WikiInterestScrappingService(WikiSite wiki)
        {
            this.wiki = wiki;
        }

        public async Task<Interest> ScrapeInterestDetails(string identifier)
        {
            await wiki.Initialization;

            var entity = new Entity(wiki, identifier);
            try
            {
                await entity.RefreshAsync(EntityQueryOptions.FetchAllProperties);
            }
            catch (OperationFailedException e)
            {
                if (e.Message.Contains("no-such-entity", StringComparison.OrdinalIgnoreCase))
                    throw new EntityNotFoundException(e.Message);
            }


            var model = new Interest()
            {
                Name = entity.Labels["en"],
                Description = entity.Descriptions["en"],
                WikiId = entity.Id,
                ImageUri = entity.ImageUris().FirstOrDefault()

            };

            string interestType = entity.InstanceOf()
                                .Intersect(IdentifierToType.Keys)
                                .FirstOrDefault();

            if (interestType == null)
                throw new AddedEntityIsNotAnInterestException(entity);

            model.Type = IdentifierToType.GetValueOrDefault(interestType);


            return model;
        }


        private readonly Dictionary<string, InterestType?>  IdentifierToType = new Dictionary<string, InterestType?>
        {
            { Books.Anthalogy, InterestType.Book },
            { Books.Book, InterestType.Book },
            { Books.EpicPoem, InterestType.Book },
            { Books.Haiku, InterestType.Book },
            { Books.LiteraryWork, InterestType.Book },
            { Books.Myth, InterestType.Book },
            { Books.Novel, InterestType.Book },
            { Books.Novella, InterestType.Book },
            { Books.Play, InterestType.Book },
            { Books.Poem, InterestType.Book },
            { Books.ShortNovel, InterestType.Book },
            { Books.ShortStory, InterestType.Book },
            { Books.WrittenWork, InterestType.Book },
            { Books.Xiaoshuo, InterestType.Book },

            { Movies.AnimatedSeries, InterestType.Movie },
            { Movies.AnimeTVSeries, InterestType.Movie },
            { Movies.FeatureFilm, InterestType.Movie },
            { Movies.Miniseries, InterestType.Movie },
            { Movies.TVSerial, InterestType.Movie },
            { Movies.TVSeries, InterestType.Movie },
            { Movies.Film3D, InterestType.Movie },
            { Movies.Film, InterestType.Movie },

            { Games.VideoGame, InterestType.Game },
            { Games.VideoGameMod, InterestType.Game },

            { Podcasts.Podcast, InterestType.Podcast },
            { Podcasts.RadioDramaSeries, InterestType.Podcast },

            { Others.CreativeWork, InterestType.Others }
        };
    }
}
