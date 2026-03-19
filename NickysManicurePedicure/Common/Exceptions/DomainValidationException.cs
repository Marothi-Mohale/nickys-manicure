namespace NickysManicurePedicure.Common.Exceptions;

public sealed class DomainValidationException : DomainException
{
    public DomainValidationException(
        IDictionary<string, string[]> errors,
        string message = "One or more validation errors occurred.",
        string errorCode = "validation_error")
        : base(
            message,
            StatusCodes.Status400BadRequest,
            errorCode,
            "Validation failed.")
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; }
}
