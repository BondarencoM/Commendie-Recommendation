using System;

namespace RecommendationService.Models.Exceptions;

public class AddedEntityIsNotHumanException : Exception
{
    public AddedEntityIsNotHumanException() { }
    public AddedEntityIsNotHumanException(object obj) : base("Supplied entity was not an instance of human")
    {
        base.Data.Add("entity", obj);
    }

    public AddedEntityIsNotHumanException(string message) : base(message){}

    public AddedEntityIsNotHumanException(string message, Exception innerException) : base(message, innerException){}
}
