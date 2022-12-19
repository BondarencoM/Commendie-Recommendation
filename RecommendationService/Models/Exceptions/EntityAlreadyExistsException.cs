using System;

namespace RecommendationService.Models.Exceptions;

public class EntityAlreadyExistsException<T> : Exception
{
    public T Entity { get; set; }

    public EntityAlreadyExistsException(T entity)
    {
        Entity = entity;
    }

}
