using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Application.Services;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Infrastructure;
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
            DatabaseProviderConfigurator.Configure(
                options,
                normalizedProvider,
                connectionString,
                environment.ContentRootPath);
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddProblemDetails();
        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Scoped);
        services.AddFluentValidationAutoValidation(options =>
        {
            options.DisableDataAnnotationsValidation = false;
        });
        services.AddHealthChecks()
            .AddCheck(
                "self",
                () => HealthCheckResult.Healthy("Application is running."),
                tags: ["live"])
            .AddDbContextCheck<ApplicationDbContext>(
                "database",
                tags: ["ready", "db"]);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Nicky's Manicure & Pedicure API",
                Version = "v1",
                Description = "Production-minded backend endpoints for the public luxury salon website and future admin use."
            });
        });

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestMethod
                | HttpLoggingFields.RequestPath
                | HttpLoggingFields.ResponseStatusCode
                | HttpLoggingFields.Duration;
            options.CombineLogs = true;
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddControllersWithViews(options =>
        {
            var defaultJsonFormatter = options.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()
                .FirstOrDefault();

            if (defaultJsonFormatter is not null)
            {
                options.OutputFormatters.Remove(defaultJsonFormatter);
            }

            options.OutputFormatters.Insert(0, new SystemTextJsonStreamOutputFormatter());
        });
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Instance = context.HttpContext.Request.Path
                };

                problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                problemDetails.Extensions["correlationId"] = context.HttpContext.Response.Headers["X-Correlation-ID"].ToString();
                problemDetails.Extensions["errorCode"] = "validation_error";

                return new BadRequestObjectResult(problemDetails);
            };
        });

        services.AddScoped<IInquiryService, InquiryService>();
        services.AddScoped<IBookingRequestService, BookingRequestService>();
        services.AddScoped<IBookingPublicCommandService, PublicBookingCommandService>();
        services.AddScoped<IBookingAdminService, BookingAdminService>();
        services.AddSingleton<IBookingNotificationService, NullBookingNotificationService>();
        services.AddScoped<IContactInquiryPublicCommandService, PublicContactInquiryCommandService>();
        services.AddScoped<IContactInquiryAdminService, ContactInquiryAdminService>();
        services.AddSingleton<IContactInquiryNotificationService, NullContactInquiryNotificationService>();
        services.AddScoped<IPublicSalonApiService, PublicSalonApiService>();

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

}
