using System.Diagnostics.CodeAnalysis;

namespace Telosatis.Common;

public record Result
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;
    
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));

    public static implicit operator Result(Error error) => new(false, error);
}

public record Result<T> : Result
{
    private readonly T? _value;

    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access Value of a failed result.");

    private Result(T value) : base(true, null)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
        _value = default;
    }
    
    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Error error) => new(error);

    public static explicit operator T(Result<T> result) => result.Value;
}