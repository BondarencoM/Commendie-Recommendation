namespace RecommendationService.Models
{
    public class Interest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageURI { get; set; }
        public InterestType Type { get; set; }
    }

    public enum InterestType
    {
        Book, Movie, Game
    }
}
