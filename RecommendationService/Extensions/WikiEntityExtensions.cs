using RecommendationService.Models.Wikibase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Wikibase;

namespace RecommendationService.Extensions
{
    public static class WikiEntityExtensions
    {
        public static bool IsHuman(this IEntity entity)
        {
            return entity.InstanceOf().Contains("Q5");  // Q5 is the identifier for 'human'
        }

        public static IEnumerable<string> InstanceOf(this IEntity entity)
        {
            if (entity.Claims.Count == 0)
                return null;

            return entity.Claims[WikibaseProperty.InstanceOf]
                .Select(c => c.MainSnak.DataValue.ToString());
        }

        public static IEnumerable<Uri> ImageUris(this IEntity entity)
        {
            return entity.Claims[WikibaseProperty.Image]
                .Select( c => new Uri($"https://commons.wikimedia.org/wiki/Special:FilePath/{c.MainSnak.DataValue}"));
        }

        public static Uri WikipediaLink (this IEntity entity)
        {
            try
            {
                var title = entity.SiteLinks[WikiSites.EnglishWikipedia]?.Title?.Replace(' ', '_');
                return new Uri("https://en.wikipedia.org/wiki/" + title);
            }
            catch(KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
