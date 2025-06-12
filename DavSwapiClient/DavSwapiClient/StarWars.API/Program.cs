using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Config;
using StarWars.Domain.Interfaces.Caching;
using StarWars.Domain.Interfaces.Manager;
using StarWars.Domain.Interfaces.Security;
using StarWars.Infrastructure.Caching;
using StarWars.Infrastructure.Data;
using StarWars.Infrastructure.External;
using StarWars.Infrastructure.Manager;
using StarWars.Infrastructure.Services.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<SwapiSettings>(builder.Configuration.GetSection("SwapiSettings")); 

builder.Services.AddDbContext<StarWarsDbContext>((options) => options.UseInMemoryDatabase("StarWarsDB"));

builder.Services.AddSingleton<ISwapiClient, SwapiClient>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IStarshipManager, StarshipManager>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", options =>
{
    var settings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    if (settings == null || string.IsNullOrEmpty(settings.Key)) throw new ApplicationException("Jwt settings cannot be null");

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = settings.Issuer,
        ValidAudience = settings.Audience,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.Key))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StarWarsDbContext>();
    DbSeeder.SeedDefaultUser(db);
}

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
