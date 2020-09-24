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
                new Persona { Id = 3, Name="Jon Skeet" },
                new Persona { Id = 4, Name="John Green" },
                new Persona { Id = 5, Name="Grant Sanders" },
                new Persona { Id = 6, Name="Bill Gates" },
                new Persona { Id = 7, Name="Steve Jobs" }
                );

            modelBuilder.Entity<Interest>().HasData(
                new Interest { Id = 1, Name = "So good they can't ignore you", Type = InterestType.Book},
                new Interest { Id = 2, Name = "Superintelligence", Type = InterestType.Book},
                new Interest { Id = 3, Name = "Into thin air", Type = InterestType.Book},
                new Interest { Id = 4, Name = "Titanic", Type = InterestType.Movie},
                new Interest { Id = 5, Name = "Brief history of time", Type = InterestType.Book},
                new Interest { Id = 6, Name = "Factorio", Type = InterestType.Game},
                new Interest { Id = 7, Name = "Mario kart", Type = InterestType.Game},
                new Interest { Id = 8, Name = "So you've been publicly shame", Type = InterestType.Game},
                new Interest { Id = 9, Name = "Inside Bill's head", Type = InterestType.Movie},
                new Interest { Id = 10, Name = "Hamlet", Type = InterestType.Book}
                );

            modelBuilder.Entity<Recommendation>().HasData(
                new Recommendation { Id = 11, PersonaId = 1, InterestId = 1 },
                new Recommendation { Id = 12, PersonaId = 1, InterestId = 2 },
                new Recommendation { Id = 13, PersonaId = 1, InterestId = 3 },
                new Recommendation { Id = 14, PersonaId = 1, InterestId = 5 },
                new Recommendation { Id = 15, PersonaId = 1, InterestId = 6 },
                new Recommendation { Id = 16, PersonaId = 1, InterestId = 7 },
                new Recommendation { Id = 17, PersonaId = 1, InterestId = 8 },
                new Recommendation { Id = 18, PersonaId = 1, InterestId = 9 },

                new Recommendation { Id = 21, PersonaId = 2, InterestId = 3 },
                new Recommendation { Id = 22, PersonaId = 2, InterestId = 4 },
                new Recommendation { Id = 23, PersonaId = 2, InterestId = 5 },
                new Recommendation { Id = 24, PersonaId = 2, InterestId = 9 },

                new Recommendation { Id = 31, PersonaId = 3, InterestId = 2 },
                new Recommendation { Id = 32, PersonaId = 3, InterestId = 7 },
                new Recommendation { Id = 33, PersonaId = 3, InterestId = 9 },

                new Recommendation { Id = 41, PersonaId = 4, InterestId = 1 },
                new Recommendation { Id = 42, PersonaId = 4, InterestId = 10 },

                new Recommendation { Id = 51, PersonaId = 5, InterestId = 2 },
                new Recommendation { Id = 52, PersonaId = 5, InterestId = 4 },
                new Recommendation { Id = 53, PersonaId = 5, InterestId = 5 },
                new Recommendation { Id = 54, PersonaId = 5, InterestId = 6 },

                new Recommendation { Id = 61, PersonaId = 6, InterestId = 5 },
                new Recommendation { Id = 62, PersonaId = 6, InterestId = 9 }


                );

        }
    }
}