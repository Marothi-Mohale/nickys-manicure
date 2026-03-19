using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Common.Exceptions;

namespace NickysManicurePedicure.Infrastructure;

public static class ApiProblemDetailsFactory
{
    public static ProblemDetails CreateForException(HttpContext context, Exception exception, bool includeExceptionDetails)
    {
        return exception switch
        {
            DomainValidationException validationException => CreateValidationProblemDetails(context, validationException),
            DomainException domainException => CreateDomainProblemDetails(context, domainException),
            _ => CreateUnexpectedProblemDetails(context, exception, includeExceptionDetails)
        };
    }

    public static ProblemDetails CreateForStatusCode(HttpContext context)
    {
        var (title, detail, type, errorCode) = context.Response.StatusCode switch
        {
            StatusCodes.Status400BadRequest => ("Bad request.", "The request could not be processed.", "https://tools.ietf.org/html/rfc9110#section-15.5.1", "bad_request"),
            StatusCodes.Status401Unauthorized => ("Unauthorized.", "Authentication is required to access this resource.", "https://tools.ietf.org/html/rfc9110#section-15.5.2", "unauthorized"),
            StatusCodes.Status403Forbidden => ("Forbidden.", "You do not have permission to access this resource.", "https://tools.ietf.org/html/rfc9110#section-15.5.4", "forbidden"),
            StatusCodes.Status404NotFound => ("Resource not found.", "The requested API resource was not found.", "https://tools.ietf.org/html/rfc9110#section-15.5.5", "resource_not_found"),
            StatusCodes.Status405MethodNotAllowed => ("Method not allowed.", "The requested HTTP method is not supported for this endpoint.", "https://tools.ietf.org/html/rfc9110#section-15.5.6", "method_not_allowed"),
            StatusCodes.Status409Conflict => ("Conflict.", "The request could not be completed because of a conflict.", "https://tools.ietf.org/html/rfc9110#section-15.5.10", "conflict"),
            _ => ("Request failed.", "The request could not be completed.", "https://tools.ietf.org/html/rfc9110", "request_failed")
        };

        return CreateProblemDetails(
            context,
            context.Response.StatusCode,
            title,
            detail,
            type,
            errorCode);
    }

    private static ProblemDetails CreateValidationProblemDetails(HttpContext context, DomainValidationException exception)
    {
        var problemDetails = new ValidationProblemDetails(exception.Errors)
        {
            Status = exception.StatusCode,
            Title = exception.Title,
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Instance = context.Request.Path
        };

        Enrich(problemDetails, context, exception.ErrorCode);
        return problemDetails;
    }

    private static ProblemDetails CreateDomainProblemDetails(HttpContext context, DomainException exception)
    {
        var type = exception.StatusCode switch
        {
            StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            StatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            _ => "https://tools.ietf.org/html/rfc9110"
        };

        return CreateProblemDetails(
            context,
            exception.StatusCode,
            exception.Title ?? "Request failed.",
            exception.Message,
            type,
            exception.ErrorCode);
    }

    private static ProblemDetails CreateUnexpectedProblemDetails(HttpContext context, Exception exception, bool includeExceptionDetails)
    {
        var problemDetails = CreateProblemDetails(
            context,
            StatusCodes.Status500InternalServerError,
            "An unexpected error occurred.",
            "The server could not complete the request. Please try again later.",
            "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            "unexpected_error");

        if (includeExceptionDetails)
        {
            problemDetails.Extensions["exceptionType"] = exception.GetType().Name;
            problemDetails.Extensions["exceptionMessage"] = exception.Message;
        }

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        int status,
        string title,
        string detail,
        string type,
        string errorCode)
    {
        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = type,
            Instance = context.Request.Path
        };

        Enrich(problemDetails, context, errorCode);
        return problemDetails;
    }

    private static void Enrich(ProblemDetails problemDetails, HttpContext context, string errorCode)
    {
        var correlationId = context.Response.Headers.TryGetValue("X-Correlation-ID", out var headerValue)
            ? headerValue.ToString()
            : context.TraceIdentifier;

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["correlationId"] = correlationId;
        problemDetails.Extensions["errorCode"] = errorCode;
    }
}
