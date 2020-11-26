using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Exceptions
{
    public class AddedEntityIsNotAnInterest : Exception
    {
        public AddedEntityIsNotAnInterest(object obj) : base("Supplied entity was not a valid interest")
        {
            Data["entity"] = obj;
        }
    }
}
