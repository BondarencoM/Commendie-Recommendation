
using CommentService.Extensions;
using CommentService.Models;
using CommentService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Services:AuthenticationService"];

        options.Audience = "comment-service";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Services:ExternalAuthenticationService"],
            NameClaimType = "name",
        };
       
    });

builder.Services.AddAuthorization();

builder.Services.AddRateLimiting();

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
builder.Services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddTransient<ICommentService, CommentService.Services.CommentService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(@"Data Source=Comments.db"));
}
else
{
    var conString = builder.Configuration.GetConnectionString("AzureConnection");
    builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(conString));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}

app.UseCors(MyAllowSpecificOrigins);

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();