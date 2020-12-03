using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ProfileService.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserProfile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            long ids = 1;
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile() { Id = ids++, Username = "@root" }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        }
    }
}