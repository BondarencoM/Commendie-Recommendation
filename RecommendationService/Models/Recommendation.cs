using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models
{
    public class Recommendation
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public long PersonId { get; set; }
        public long ProductId { get; set; }

        public Persona Persona { get; set; }
        public Interest Product { get; set; }
    }
}
