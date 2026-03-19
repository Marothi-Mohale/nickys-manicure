namespace NickysManicurePedicure.Common.Errors;

public sealed class ErrorDetails
{
    public required string Code { get; init; }
    public required string Message { get; init; }

    public static ErrorDetails Unexpected(string message) => new()
    {
        Code = "unexpected_error",
        Message = message
    };

    public static ErrorDetails Validation(string message) => new()
    {
        Code = "validation_error",
        Message = message
    };
}
