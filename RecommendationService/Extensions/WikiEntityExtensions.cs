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
        public static bool IsHuman(this Entity entity)
        {
            return entity.InstanceOf() == "Q5";  // Q5 is the identifier for 'human'
        }

        public static string InstanceOf(this Entity entity)
        {
            if (entity.Claims.Count == 0)
                return null;

            return entity.Claims[WikibaseProperty.InstanceOf]
                        .FirstOrDefault()
                        ?.MainSnak.DataValue.ToString() ?? null;
        }

        public static IEnumerable<string> ImageUris (this Entity entity)
        {
            return entity.Claims[WikibaseProperty.Image]
                .Select( c => $"https://commons.wikimedia.org/wiki/Special:FilePath/{c.MainSnak.DataValue}");
        }

        public static string WikipediaLink (this Entity entity)
        {
            return "https://en.wikipedia.org/wiki/" + entity.SiteLinks[WikiSites.EnglishWikipedia].Title;
        }
    }
}
