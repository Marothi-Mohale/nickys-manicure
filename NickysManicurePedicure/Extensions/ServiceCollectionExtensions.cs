using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.Services;

namespace NickysManicurePedicure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<BusinessProfileOptions>()
            .Bind(configuration.GetSection(BusinessProfileOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddApplicationData(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var databaseOptions = configuration
            .GetSection(DatabaseOptions.SectionName)
            .Get<DatabaseOptions>() ?? new DatabaseOptions();
        var normalizedProvider = databaseOptions.Provider.Trim().ToLowerInvariant();
        var connectionString = ResolveConnectionString(configuration, normalizedProvider);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            switch (normalizedProvider)
            {
                case DatabaseProviders.Sqlite:
                    EnsureSqliteDataDirectoryExists(environment.ContentRootPath, connectionString);
                    options.UseSqlite(connectionString);
                    break;
                case DatabaseProviders.PostgreSql:
                    options.UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure());
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported database provider '{databaseOptions.Provider}'. Supported values: Sqlite, PostgreSql.");
            }
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestMethod
                | HttpLoggingFields.RequestPath
                | HttpLoggingFields.ResponseStatusCode
                | HttpLoggingFields.Duration;
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services
            .AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

        services.AddScoped<IInquiryService, InquiryService>();
        services.AddScoped<IBookingRequestService, BookingRequestService>();

        return services;
    }

    private static string ResolveConnectionString(IConfiguration configuration, string provider) =>
        provider switch
        {
            DatabaseProviders.Sqlite => configuration.GetConnectionString("SqliteConnection")
                ?? Environment.GetEnvironmentVariable("ConnectionStrings__SqliteConnection")
                ?? throw new InvalidOperationException(
                    "A SQLite connection string is required. Set ConnectionStrings:SqliteConnection."),
            DatabaseProviders.PostgreSql => Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSqlConnection")
                ?? configuration.GetConnectionString("PostgreSqlConnection")
                ?? Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? throw new InvalidOperationException(
                    "A PostgreSQL connection string is required. Set ConnectionStrings:PostgreSqlConnection or DATABASE_URL."),
            _ => throw new InvalidOperationException($"Unsupported database provider '{provider}'.")
        };

    private static void EnsureSqliteDataDirectoryExists(string contentRootPath, string connectionString)
    {
        const string prefix = "Data Source=";

        if (!connectionString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var databasePath = connectionString[prefix.Length..].Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(databasePath) || databasePath == ":memory:" || Path.IsPathRooted(databasePath))
        {
            return;
        }

        var fullPath = Path.GetFullPath(databasePath, contentRootPath);
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
