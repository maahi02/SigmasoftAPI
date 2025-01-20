using Domain.Models;
using Infrastructure.Content;
using Infrastructure.Content.Data;
using Infrastructure.Content.Repository;
using Infrastructure.Content.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository<Candidate>, CandidateRepository>();
builder.Services.AddScoped<CandidateService>();


builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

var app = builder.Build();


// Migration run on start
using (IServiceScope? scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();
    try
    {
        // Apply migrations for AppDbContext
        var appDbContext = service.GetRequiredService<AppDbContext>();
        await appDbContext.Database.MigrateAsync();

    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("*");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();