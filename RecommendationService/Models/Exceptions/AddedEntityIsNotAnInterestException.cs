using System;

namespace RecommendationService.Models.Exceptions
{
    public class AddedEntityIsNotAnInterestException : Exception
    {
        public AddedEntityIsNotAnInterestException(object obj) : base("Supplied entity was not a valid interest")
        {
            Data["entity"] = obj;
        }

        public AddedEntityIsNotAnInterestException(){}

        public AddedEntityIsNotAnInterestException(string message) : base(message){}

        public AddedEntityIsNotAnInterestException(string message, Exception innerException) : base(message, innerException){}
    }
}
