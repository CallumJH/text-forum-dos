using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Whispers.Chat.Infrastructure.Data;
using Whispers.Chat.Infrastructure.Data.Identity;
using Whispers.Chat.Infrastructure.Data.Moderation;
using Whispers.Chat.Infrastructure.Data.Posts;
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
      sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(sqliteDbConnectionString));

builder.Services.AddDbContext<PostsContext>(options =>
    options.UseSqlite(sqliteDbConnectionString));

builder.Services.AddDbContext<ModerationContext>(options =>
    options.UseSqlite(sqliteDbConnectionString));

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlite(sqliteDbConnectionString));

var app = builder.Build();

await app.UseAppMiddlewareAndSeedDatabase();

// Register minimal API endpoints
//app.MapUserEndpoints();

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
