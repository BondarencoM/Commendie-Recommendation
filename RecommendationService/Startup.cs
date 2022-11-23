using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using RecommendationService.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using RecommendationService.Services.Interfaces;
using RecommendationService.Services;
using WikiClientLibrary.Client;
using WikiClientLibrary.Sites;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using RecommendationService.Configs;
using RecommendationService.Extensions;

namespace RecommendationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(@"Data Source=recommendations.db"));
            services.AddControllers();

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = Configuration["Services:AuthenticationService"];
                // Todo: HUGE PROBLEM, MAKE HHTTPS WORK somehow
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = "recommendation-service",
                    // HUGE PROBLEM?
                    ValidateIssuer = false,
                    NameClaimType = "name",
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddHttpContextAccessor();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);

            services.AddScoped( 
                sv => new WikiClient{ ClientUserAgent = "WCLQuickStart/1.0 bondarencom" }
            );
            services.AddScoped( 
                sv => new WikiSite(sv.GetService<WikiClient>(), "https://www.wikidata.org/w/api.php")
            );

            services.AddScoped<IPersonaScrappingService, WikiPersonaScrappingService>();
            services.AddScoped<IInterestScrappingService, WikiInterestScrappingService>();

            services.AddScoped<IPersonasService, PersonasService>();
            services.AddScoped<IInterestService, InterestService>();
            services.AddScoped<IRecommendationService, RecommednationService>();

            services.AddTransient<ICommentService, CommentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            app.UseCors("default");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRabbitMQ();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.MigrateDatabase();

        }

    }
}
