using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProfileService.Profiles;

public class ProfileService : IProfileService
{
    private readonly ILogger<ProfileService> logger;
    private readonly DatabaseContext db;

    public ProfileService(
        ILogger<ProfileService> logger,
        DatabaseContext db)
    {
        this.logger = logger;
        this.db = db;
    }

    public async Task<Profile> FindByUsername(string username) =>
        await db.Profiles
            .Where(p => p.Id == username)
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
            .FirstOrDefaultAsync()
            ?? throw new ProfileNotFoundException();

    public Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Body.ToArray());
        return args.RoutingKey switch
        {
            "users.new" => AddUser(),
            _ => Default(),
        };

        async Task AddUser()
        {
            var newProfile = JsonSerializer.Deserialize<CreateProfileIM>(message) 
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(CreateProfileIM)} from {message}");

            var profile = new Profile(newProfile);

            this.db.Profiles.Add(profile);

            await this.db.SaveChangesAsync();
        }

        Task Default()
        {
            this.logger.LogWarning("Could not handle Comment message" +
                                    $" with routing key {args.RoutingKey} and body {message}");
            return Task.CompletedTask;
        }
    }
}
