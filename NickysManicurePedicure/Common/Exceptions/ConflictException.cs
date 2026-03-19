namespace NickysManicurePedicure.Common.Exceptions;

public sealed class ConflictException : DomainException
{
    public ConflictException(string message, string errorCode = "conflict", string? title = null)
        : base(
            message,
            StatusCodes.Status409Conflict,
            errorCode,
            title ?? "Conflict.")
    {
    }
}
