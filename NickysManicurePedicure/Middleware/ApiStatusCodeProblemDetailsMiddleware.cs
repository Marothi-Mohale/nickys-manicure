using System.Text.Json;
using NickysManicurePedicure.Infrastructure;

namespace NickysManicurePedicure.Middleware;

public sealed class ApiStatusCodeProblemDetailsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (!IsApiRequest(context.Request.Path)
            || context.Response.HasStarted
            || context.Response.StatusCode < StatusCodes.Status400BadRequest
            || !string.IsNullOrWhiteSpace(context.Response.ContentType)
            || context.Response.ContentLength.HasValue)
        {
            return;
        }

        var problemDetails = ApiProblemDetailsFactory.CreateForStatusCode(context);
        if (!context.Response.Headers.ContainsKey("X-Correlation-ID"))
        {
            context.Response.Headers["X-Correlation-ID"] = context.TraceIdentifier;
        }

        context.Response.ContentType = "application/problem+json";
        await JsonSerializer.SerializeAsync(
            context.Response.Body,
            problemDetails,
            new JsonSerializerOptions(JsonSerializerDefaults.Web),
            context.RequestAborted);
    }

    private static bool IsApiRequest(PathString path) => path.StartsWithSegments("/api");
}
