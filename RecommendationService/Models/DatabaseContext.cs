using Microsoft.EntityFrameworkCore;
using CommendMine.Models;

namespace RecommendationService.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Interest> Interests { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Recommendation> Recommendation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>().HasData(
                new Persona { Id = 1, Name="CGP Grey" },
                new Persona { Id = 2, Name="Brady Haran" },
                new Persona { Id = 3, Name="Jon Skeet" }
                );

            modelBuilder.Entity<Interest>().HasData(
                new Interest { Id = 2, Name = "So good they can't ignore you", Type = InterestType.Book},
                new Interest { Id = 1, Name = "Superintelligence", Type = InterestType.Book},
                new Interest { Id = 3, Name = "Into thin air", Type = InterestType.Book},
                new Interest { Id = 4, Name = "Titanic", Type = InterestType.Movie}
                );

            modelBuilder.Entity<Recommendation>().HasData(
                new Recommendation { Id = 1, PersonId = 1, ProductId = 1 },
                new Recommendation { Id = 2, PersonId = 1, ProductId = 2 },
                new Recommendation { Id = 3, PersonId = 1, ProductId = 3 },
                new Recommendation { Id = 4, PersonId = 2, ProductId = 3 },
                new Recommendation { Id = 5, PersonId = 2, ProductId = 4 }
                );

        }
    }
}