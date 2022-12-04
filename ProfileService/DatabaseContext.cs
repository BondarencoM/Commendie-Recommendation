using Microsoft.EntityFrameworkCore;
using ProfileService.Comments;
using ProfileService.Profiles;

namespace ProfileService;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
