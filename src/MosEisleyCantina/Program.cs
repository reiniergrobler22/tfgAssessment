using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MosEisleyCantina.MosEisleyCantina.DataAccess;
using MosEisleyCantina.MosEisleyCantina.Domain;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AspNetCoreRateLimit; // Import rate limiting package

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register memory cache
builder.Services.AddMemoryCache();

// Setup Swagger, CORS, Authentication, Rate Limiting, and Database Context
AddSwagger(builder);
AddCors(builder);
AddAuthentication(builder);
AddRateLimiting(builder);
AddDbContexts(builder);

// Add services for application use cases
builder.Services.AddScoped<ICantinaService, CantinaService>();

var app = builder.Build();

// Use global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Use CORS and Rate Limiting
app.UseCors("AllowAll");
app.UseIpRateLimiting(); // Apply rate limiting middleware

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthentication(); // Ensure this comes before Authorization middleware
app.UseAuthorization();
app.MapControllers();
app.Run();

static void AddAuthentication(WebApplicationBuilder builder)
{
    // Configure Google OAuth2 Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"];
        options.ClientSecret = builder.Configuration["Google:ClientSecret"];
        options.Scope.Add("email");
        options.Scope.Add("profile");
        options.SaveTokens = true; // Saves tokens in the authentication properties
    });

    builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<CantinaDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true; // Set to false if you want to disable for new users
    });
}

static void AddSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mos Eisley Cantina API", Version = "v1" });

        // Add JWT Authentication to Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token with Bearer prefix",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, new string[] { }
            }
        });
    });
}

static void AddRateLimiting(WebApplicationBuilder builder)
{
    // Add rate limiting services
    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
    builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
}

static void AddDbContexts(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<CantinaDbContext>(options =>
    {
        // Configure SQLite with the connection string
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddScoped<ICantinaDbContext, CantinaDbContext>();
}

static void AddCors(WebApplicationBuilder builder)
{
    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });
}
