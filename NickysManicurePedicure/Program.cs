using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging.Console;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Extensions;
using NickysManicurePedicure.Infrastructure;
using NickysManicurePedicure.Middleware;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.Configure(options =>
        {
            options.ActivityTrackingOptions =
                ActivityTrackingOptions.TraceId |
                ActivityTrackingOptions.SpanId |
                ActivityTrackingOptions.ParentId;
        });
        builder.Logging.AddSimpleConsole(options =>
        {
            options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.ColorBehavior = LoggerColorBehavior.Enabled;
        });
        builder.Logging.AddDebug();

        builder.Services.AddApplicationOptions(builder.Configuration);
        builder.Services.AddApplicationData(builder.Configuration, builder.Environment);
        builder.Services.AddApplicationServices();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseForwardedHeaders();
        app.UseHttpLogging();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseStatusCodePagesWithReExecute("/status/{0}");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteAsync
        });
        app.MapHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteAsync
        });
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

        await app.Services.EnsureDatabaseReadyAsync();
        await app.RunAsync();
    }
}
