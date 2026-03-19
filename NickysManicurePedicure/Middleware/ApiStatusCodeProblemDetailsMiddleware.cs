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
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static bool IsApiRequest(PathString path) => path.StartsWithSegments("/api");
}
