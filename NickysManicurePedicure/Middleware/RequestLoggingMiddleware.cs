using System.Diagnostics;

namespace NickysManicurePedicure.Middleware;

public sealed class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldSkipLogging(context.Request.Path))
        {
            await next(context);
            return;
        }

        EnsureCorrelationIdHeader(context);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;
            var elapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            var correlationId = context.Response.Headers["X-Correlation-ID"].ToString();

            var logLevel = GetLogLevel(statusCode);
            if (logger.IsEnabled(logLevel))
            {
                logger.Log(
                    logLevel,
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs} ms. TraceIdentifier: {TraceIdentifier}. CorrelationId: {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    statusCode,
                    Math.Round(elapsedMilliseconds, 2),
                    context.TraceIdentifier,
                    correlationId);
            }
        }
    }

    private static void EnsureCorrelationIdHeader(HttpContext context)
    {
        const string headerName = "X-Correlation-ID";
        if (!context.Response.Headers.ContainsKey(headerName))
        {
            var requestedCorrelationId = context.Request.Headers[headerName].ToString().Trim();
            context.Response.Headers[headerName] = string.IsNullOrWhiteSpace(requestedCorrelationId)
                ? context.TraceIdentifier
                : requestedCorrelationId[..Math.Min(requestedCorrelationId.Length, 128)];
        }
    }

    private static LogLevel GetLogLevel(int statusCode) =>
        statusCode switch
        {
            >= 500 => LogLevel.Warning,
            >= 400 => LogLevel.Warning,
            _ => LogLevel.Information
        };

    private static bool ShouldSkipLogging(PathString path) =>
        path.StartsWithSegments("/health")
        || path.StartsWithSegments("/api/health")
        || path.StartsWithSegments("/health/live")
        || path.StartsWithSegments("/health/ready")
        || path.StartsWithSegments("/swagger")
        || path.StartsWithSegments("/css")
        || path.StartsWithSegments("/js")
        || path.StartsWithSegments("/lib")
        || path.StartsWithSegments("/images")
        || path.StartsWithSegments("/favicon.ico");
}
