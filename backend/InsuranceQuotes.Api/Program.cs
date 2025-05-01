using InsuranceQuotes.Api.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

// Load environment variables from .env.local
if (File.Exists(".env.local"))
{
    DotNetEnv.Env.Load(".env.local");
}
else
{
    Console.WriteLine("Warning: .env.local file not found");
}

var builder = WebApplication.CreateBuilder(args);

// Add environment variables as a configuration source
builder.Configuration.AddEnvironmentVariables();

// Configure authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{Environment.GetEnvironmentVariable("AZURE_AD_INSTANCE")}{Environment.GetEnvironmentVariable("AZURE_AD_TENANT_ID")}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = new[]
            {
                Environment.GetEnvironmentVariable("AZURE_AD_VALID_ISSUER_1"),
                Environment.GetEnvironmentVariable("AZURE_AD_VALID_ISSUER_2")
            },
            ValidateAudience = true,
            ValidAudiences = new[]
            {
                Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID"),
                $"api://{Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID")}",
                "api://insurance-quotes-api"
            },
            ValidateLifetime = true
        };
    });

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Insurance Quotes API",
        Version = "v1",
        Description = "API for retrieving auto insurance quotes",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@example.com"
        }
    });

    // Add security definition for JWT Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add XML comments for API documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        var allowedOrigins = builder.Configuration["AllowedOrigins"] ?? "http://localhost:3000";
        corsBuilder.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
    });
});

// Register services
builder.Services.AddSingleton<QuoteService>();

var app = builder.Build();

// Configure middleware in the correct order
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
