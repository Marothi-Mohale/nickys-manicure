namespace NickysManicurePedicure.Common.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(
        string message,
        int statusCode,
        string errorCode,
        string? title = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Title = title;
    }

    public int StatusCode { get; }

    public string ErrorCode { get; }

    public string? Title { get; }
}
