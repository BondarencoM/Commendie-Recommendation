using RecommendationService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RecommendationService.Configs;

public static class ApplicationBuilderExtensions
{
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
    }
}
