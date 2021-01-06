using Microsoft.EntityFrameworkCore;
using RecommendationService.Models.Interests;
using RecommendationService.Models.Personas;
using RecommendationService.Models.Recommendations;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationService.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Interest> Interests { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

    }
}