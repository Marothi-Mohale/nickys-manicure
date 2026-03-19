namespace NickysManicurePedicure.Common.Exceptions;

public sealed class BadRequestException : DomainException
{
    public BadRequestException(string message, string errorCode = "bad_request", string? title = null)
        : base(
            message,
            StatusCodes.Status400BadRequest,
            errorCode,
            title ?? "Bad request.")
    {
    }
}
