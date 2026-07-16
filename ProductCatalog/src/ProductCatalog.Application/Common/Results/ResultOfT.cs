using ProductCatalog.Application.Common.Errors;

namespace ProductCatalog.Application.Common.Results;

public class Result<T> : Result
{
    private readonly T? _value;

    public T? Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access value of a failed result.");

            return _value;
        }
    }

    internal Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static implicit operator Result<T>(T value) => Success(value);
}