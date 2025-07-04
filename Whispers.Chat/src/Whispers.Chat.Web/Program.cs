using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;
using Whispers.Chat.Infrastructure.Data;
using Whispers.Chat.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

var migrationsAssembly = typeof(IdentityContext).Assembly.GetName().Name;
var sqliteDbConnectionString = builder.Configuration.GetConnectionString("SqliteConnection");

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);

builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.ShortSchemaNames = true;
                });

// Configure ASP.NET Core Identity first (must match your IdentityContext generic types)
builder.Services.AddIdentity<User, Role>()
  .AddEntityFrameworkStores<IdentityContext>()
  .AddDefaultTokenProviders();

// Configure IdentityServer with both Configuration and Operational stores
builder.Services.AddIdentityServer(
  setupAction: options =>
  {
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
  })
  .AddConfigurationStore(options =>
    options.ConfigureDbContext = db => db.UseSqlite(sqliteDbConnectionString,
      sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
  .AddOperationalStore(options =>
    options.ConfigureDbContext = db => db.UseSqlite(sqliteDbConnectionString,
      sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
  .AddAspNetIdentity<User>();

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(sqliteDbConnectionString, o => o.MigrationsAssembly(migrationsAssembly)));

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlite(sqliteDbConnectionString, o => o.MigrationsAssembly(migrationsAssembly)));

var app = builder.Build();

await app.UseAppMiddlewareAndSeedDatabase();

// Register minimal API endpoints
//app.MapUserEndpoints();

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
