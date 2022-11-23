using System.ComponentModel.DataAnnotations;

namespace RecommendationService.Models.Interests
{
    public class CreateInterestInputModel
    {
        [Required]
        public string WikiId { get; set; }
    }
}
