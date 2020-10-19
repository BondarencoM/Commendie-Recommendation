using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Exceptions
{
    public class EntityAlreadyExists<T> : Exception
    {
        public T Entity { get; set; }

        public EntityAlreadyExists(T entity)
        {
            Entity = entity;
        }
    }
}
