using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Infrastructure;

namespace NickysManicurePedicure.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IWebHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context);
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        using var scope = logger.BeginScope(new Dictionary<string, object?>
        {
            ["TraceId"] = context.TraceIdentifier,
            ["CorrelationId"] = correlationId,
            ["RequestPath"] = context.Request.Path.Value,
            ["RequestMethod"] = context.Request.Method
        });

        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            logger.LogInformation(
                "Request was cancelled by the client for {Method} {Path}. TraceIdentifier: {TraceIdentifier}. CorrelationId: {CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier,
                correlationId);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            var logMessage = exception is DomainException
                ? "Handled domain exception for {Method} {Path}. TraceIdentifier: {TraceIdentifier}. CorrelationId: {CorrelationId}"
                : "Unhandled exception for {Method} {Path}. TraceIdentifier: {TraceIdentifier}. CorrelationId: {CorrelationId}";

            if (exception is DomainException)
            {
                logger.LogWarning(
                    exception,
                    logMessage,
                    context.Request.Method,
                    context.Request.Path,
                    context.TraceIdentifier,
                    correlationId);
            }
            else
            {
                logger.LogError(
                    exception,
                    logMessage,
                    context.Request.Method,
                    context.Request.Path,
                    context.TraceIdentifier,
                    correlationId);
            }

            var problemDetails = ApiProblemDetailsFactory.CreateForException(
                context,
                exception,
                environment.IsDevelopment());

            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static string ResolveCorrelationId(HttpContext context)
    {
        const string headerName = "X-Correlation-ID";
        var headerValue = context.Request.Headers[headerName].ToString().Trim();

        return string.IsNullOrWhiteSpace(headerValue)
            ? context.TraceIdentifier
            : headerValue[..Math.Min(headerValue.Length, 128)];
    }
}
