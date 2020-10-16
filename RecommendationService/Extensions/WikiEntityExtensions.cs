using RecommendationService.Models.Wikibase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            return entity.Claims
                        .Where(c => c.MainSnak.PropertyId == WikibaseProperty.InstanceOf)
                        .FirstOrDefault()
                        ?.MainSnak.DataValue.ToString() ?? null;
        }
    }
}
