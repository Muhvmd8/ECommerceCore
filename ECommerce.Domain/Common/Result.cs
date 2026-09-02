namespace ECommerce.Domain.Common;
public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("A successful result cannot have an error.");

        if (!isSuccess && error is null)
            throw new InvalidOperationException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error? Error { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, null);

    public static Result Failure(Error error) => new(false, error);
}


public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;
    private Result(bool isSuccess, TValue? value, Error? error = null)
        : base(isSuccess, error) => _value = value;

    public TValue Value { get; }
    public static Result<TValue> Success(TValue value) => new(true, value, null);
    public static Result<TValue> Failure(Error error) => new(false, default, error);

}