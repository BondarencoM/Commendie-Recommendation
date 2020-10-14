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

            if (entity.Claims.Count == 0)
                return false;

            string type = entity.Claims
                            .Where(c => c.MainSnak.PropertyId == WikibaseProperty.InstanceOf)
                            .Select(c => c.MainSnak.DataValue)
                            .Single()
                            .ToString();

            return type == "Q5"; // Q5 is the identifier for 'human'
        }
    }
}
