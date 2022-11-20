using Microsoft.EntityFrameworkCore;

namespace CommentService.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Comment> Comments { get; set; }
}
