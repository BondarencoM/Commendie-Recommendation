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
            Database.EnsureCreated();
        }

        public DbSet<Interest> Interests { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Recommendation> Recommendation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>().HasData(new []{
                new Persona { Id = 1, WikiId = "Q5006102",    Name = "CGP Grey", },
                new Persona { Id = 2, WikiId = "Q4955182",    Name = "Brady Haran" },
                new Persona { Id = 3, WikiId = "Q95137087",   Name = "Jon Skeet" },
                new Persona { Id = 4, WikiId = "Q630446",     Name = "John Green" },
                new Persona { Id = 5, WikiId = "Q19837",      Name = "Steve Jobs" },
                new Persona { Id = 6, WikiId = "Q5284",       Name = "Bill Gates" },
                }.Select( p => {
                    p.AddedBy = "@xtronik";
                    return p;
                }));

            modelBuilder.Entity<Interest>().HasData(new[] {
                new Interest { Id = 1,  WikiId = "Q96653616",   Name = "So good they can't ignore you", Type = InterestType.Book},
                new Interest { Id = 2,  WikiId = "Q18386449",   Name = "Superintelligence",             Type = InterestType.Book},
                new Interest { Id = 3,  WikiId = "Q851767",     Name = "Into thin air",                 Type = InterestType.Book},
                new Interest { Id = 4,  WikiId = "Q44578",      Name = "Titanic",                       Type = InterestType.Movie},
                new Interest { Id = 5,  WikiId = "Q471726",     Name = "Brief history of time",         Type = InterestType.Book},
                new Interest { Id = 6,  WikiId = "Q16972008",   Name = "Factorio",                      Type = InterestType.Game},
                new Interest { Id = 7,  WikiId = "Q13427106",   Name = "Mario kart 8",                  Type = InterestType.Game},
                new Interest { Id = 8,  WikiId = "Q19895954",   Name = "So you've been publicly shame", Type = InterestType.Game},
                new Interest { Id = 9,  WikiId = "Q68319034",   Name = "Inside Bill's brain",           Type = InterestType.Movie},
                new Interest { Id = 10, WikiId = "Q41567",      Name = "Hamlet",                        Type = InterestType.Book},
            }.Select(i => { 
                i.AddedBy = "@xtronik";
                return i; 
            }));

            var recommendations = new List<Recommendation>
            {
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
            };
            recommendations = recommendations.Select(r => { 
                r.Context = "https://example.com/url-with-proof";
                r.AddedBy = "@xtronik";
                return r; 
            }).ToList();

            modelBuilder.Entity<Recommendation>().HasData(recommendations);

        }
    }
}