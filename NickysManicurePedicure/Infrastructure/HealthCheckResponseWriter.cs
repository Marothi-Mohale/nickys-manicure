using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NickysManicurePedicure.Infrastructure;

public static class HealthCheckResponseWriter
{
    public static async Task WriteAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = report.Status == HealthStatus.Healthy
            ? StatusCodes.Status200OK
            : StatusCodes.Status503ServiceUnavailable;

        var payload = new
        {
            traceId = context.TraceIdentifier,
            correlationId = context.Response.Headers["X-Correlation-ID"].ToString(),
            timestampUtc = DateTime.UtcNow,
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            entries = report.Entries.ToDictionary(
                pair => pair.Key,
                pair => new
                {
                    status = pair.Value.Status.ToString(),
                    description = pair.Value.Description,
                    duration = pair.Value.Duration.TotalMilliseconds,
                    tags = pair.Value.Tags,
                    data = pair.Value.Data
                })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
