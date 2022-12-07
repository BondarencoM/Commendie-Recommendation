using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ProfileService;
using ProfileService.Comments;
using ProfileService.Extensions;
using ProfileService.Profiles;
using System.Security.Principal;


var builder = WebApplication.CreateBuilder(args);
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Services:AuthenticationService"];

        options.Audience = "profile-service";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Services:ExternalAuthenticationService"],
            NameClaimType = "name",
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User!);

builder.Services.AddScoped<IProfileService, ProfileService.Profiles.ProfileService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(@"Data Source=Profiles.db"));
}
else
{
    var conString = builder.Configuration.GetConnectionString("AzureConnection");
    builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(conString));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}
app.UseCors(MyAllowSpecificOrigins);

app.UseRabbitMQ();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
