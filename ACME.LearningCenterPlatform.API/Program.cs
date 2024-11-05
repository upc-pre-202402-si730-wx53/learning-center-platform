using ACME.LearningCenterPlatform.API.Profiles.Application.Internal.CommandServices;
using ACME.LearningCenterPlatform.API.Profiles.Application.Internal.QueryServices;
using ACME.LearningCenterPlatform.API.Profiles.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Profiles.Domain.Services;
using ACME.LearningCenterPlatform.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using ACME.LearningCenterPlatform.API.Profiles.Interfaces.ACL;
using ACME.LearningCenterPlatform.API.Publishing.Application.Internal.CommandServices;
using ACME.LearningCenterPlatform.API.Publishing.Application.Internal.OutboundServices.ACL;
using ACME.LearningCenterPlatform.API.Publishing.Application.Internal.QueryServices;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Services;
using ACME.LearningCenterPlatform.API.Publishing.Infrastructure.Persistence.EFC.Repositories;
using ACME.LearningCenterPlatform.API.Shared.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.LearningCenterPlatform.API.Shared.Interfaces.ASP.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Verify Database Connection String
if (connectionString is null)
    // Stop the application id the connection string is not set.
    throw new Exception("Database connection string is not set.");

// Configure Database Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
        });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());

// Configure Lower Case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Publishing Bounded Context
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryCommandService, CategoryCommandService>();
builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();

builder.Services.AddScoped<ITutorialRepository, TutorialRepository>();
builder.Services.AddScoped<ITutorialCommandService, TutorialCommandService>();
builder.Services.AddScoped<ITutorialQueryService, TutorialQueryService>();


builder.Services.AddScoped<IExternalProfileService, ExternalProfileService>();




// Profile Bounded Context
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}


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
