namespace ECommerce.Domain.Common;

public class Result
{
    protected Result(bool isSuccess, Error? error = null)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("Success cannot has an error.");

        if (!isSuccess && error is null)
            throw new InvalidOperationException("Success must has an error.");

        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public Error? Error { get; }
    public bool IsFailure => !IsSuccess;


    public static Result Success()
        => new Result(true);
    public static Result Failure(Error error)
        => new Result(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    private Result(TValue? value,
        bool isSuccess,
        Error? error = null)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "Cannot access the value of a failed result.");
    public static Result<TValue> Success(TValue value)
        => new Result<TValue>(value, true);
    public new static Result<TValue> Failure(Error error)
        => new Result<TValue>(default, false, error);

    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(_value!)
            : onFailure(Error!);
    }
    public static implicit operator Result<TValue>(TValue value) =>
        Success(value);
}
