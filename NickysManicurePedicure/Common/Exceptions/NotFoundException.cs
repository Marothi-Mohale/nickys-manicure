namespace NickysManicurePedicure.Common.Exceptions;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(string resourceName, object? resourceId = null, string? message = null, string errorCode = "resource_not_found")
        : base(
            message ?? BuildMessage(resourceName, resourceId),
            StatusCodes.Status404NotFound,
            errorCode,
            "Resource not found.")
    {
    }

    private static string BuildMessage(string resourceName, object? resourceId) =>
        resourceId is null
            ? $"{resourceName} was not found."
            : $"{resourceName} '{resourceId}' was not found.";
}
