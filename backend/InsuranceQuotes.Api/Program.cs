using InsuranceQuotes.Api.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

    // Add XML comments for API documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Register services
builder.Services.AddSingleton<QuoteService>();

var app = builder.Build();

// Configure middleware in the correct order
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance Quotes API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseRouting();
app.UseCors();

// Configure other middleware
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
