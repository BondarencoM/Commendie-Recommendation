using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Exceptions
{
    public class AddedEntityIsNotHuman : Exception
    {
        public AddedEntityIsNotHuman() { }
        public AddedEntityIsNotHuman(object obj) : base("Supplied entity was not an instance of human")
        {
            base.Data.Add("entity", obj);
        }
    }
}
