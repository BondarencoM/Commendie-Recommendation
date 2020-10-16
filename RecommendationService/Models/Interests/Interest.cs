using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecommendationService.Models.Interests
{
    public class Interest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WikiId { get; set; }



        [NotMapped]
        [JsonIgnore]
        public InterestType? Type 
        {
            get => string.IsNullOrEmpty(TypeString) ? null : Enum.Parse(typeof(InterestType), TypeString) as InterestType?;
            set => TypeString = value.ToString();
        }

        [JsonPropertyName("type")]
        public string TypeString { get; private set; }
    }

    public enum InterestType
    {
        Book, Movie, Game,
        Podcast,
        Others
    }
}
