using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models
{
    public class UserProfile
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
        
    }
}
