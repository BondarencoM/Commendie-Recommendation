using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace AuthenticationService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public DbSet<DownloadablePersonalData> DownloadablePersonalDatas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DownloadablePersonalData>()
               .HasOne(d => d.ApplicationUser)
               .WithMany(u => u.DownloadablePersonalDatas)
               .HasForeignKey(d => d.ApplicationUserName)
               .HasPrincipalKey(u => u.UserName)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
