using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieCatalogApi.Configurations;
using MovieCatalogApi.Conmfigurations;
using MovieCatalogApi.Models;
using MovieCatalogApi.Services;
using MovieCatalogApi.Services.CustomAuth;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:63343")
                .AllowAnyHeader()
                .AllowAnyMethod();
            policy.WithOrigins("http://127.0.0.1:5500")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IFavoriteMoviesService, FavoriteMoviesService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHostedService<TokenCleanerService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IAuthorizationHandler, ValidateTokenRequirementHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization(options =>
{
      options.AddPolicy(
          "ValidateToken",
          policy => policy.Requirements.Add(new ValidateTokenRequirement()));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtConfigurations.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtConfigurations.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = JwtConfigurations.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

//DB connection
var connectionPsql = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionPsql));

var app = builder.Build();

//BD init and update
using var serviceScope = app.Services.CreateScope();
var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
dbContext?.Database.Migrate();


app.UseExceptionHandlingMiddlwares();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();