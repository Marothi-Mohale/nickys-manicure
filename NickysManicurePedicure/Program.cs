using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Extensions;
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
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
            app.UseHsts();
        }

        app.UseForwardedHeaders();
        app.UseHttpLogging();
        app.UseStatusCodePagesWithReExecute("/status/{0}");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        await app.Services.EnsureDatabaseReadyAsync();
        await app.RunAsync();
    }
}
