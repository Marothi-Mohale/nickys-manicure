using NickysManicurePedicure.Common.Errors;

namespace NickysManicurePedicure.Common.Results;

public class Result
{
    protected Result(bool isSuccess, ErrorDetails? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public ErrorDetails? Error { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(ErrorDetails error) => new(false, error);
}

public sealed class Result<T> : Result
{
    private Result(bool isSuccess, T? value, ErrorDetails? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(true, value, null);

    public static new Result<T> Failure(ErrorDetails error) => new(false, default, error);
}
