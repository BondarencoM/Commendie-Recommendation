using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RecommendationService.Models.Interests
{
    public class Interest
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public string WikiId { get; set; }

        public Uri ImageUri { get; set; }

        [Required]
        public string AddedBy { get; set; }

        [NotMapped]
        [JsonIgnore]
        public InterestType? Type 
        {
            get => string.IsNullOrEmpty(TypeString) ? null : Enum.Parse(typeof(InterestType), TypeString) as InterestType?;
            set => TypeString = value.ToString();
        }

        [JsonPropertyName("type")]
        [Required]
        public string TypeString { get; private set; }
    }

    public enum InterestType
    {
        Book, Movie, Game,
        Podcast,
        Others
    }
}
