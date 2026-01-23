using admin.Core.Enums;
using admin.Core.Interfaces;

namespace admin.Core.Model;

public sealed class Result : IResultBase
{
    public bool IsSuccess => Error is null;
    public ResultErrorCode? Error { get; }
    public string? Message { get; }
    internal Result(ResultErrorCode? errorCode = null, string? message = null)
    {
        Error = errorCode;
        Message = message;
    }
    public static Result Success() => new();
    public static Result Failed(ResultErrorCode errorCode, string? message) => new(errorCode, message);

    //пока что так
    public static Result Failed(string? message) => new(ResultErrorCode.Unknown, message);
}
public sealed class Result<T> : IResultBase
{
    public Result(T value)
    {
        Value = value;
    }
    public T Value { get; }
    public bool IsSuccess => Error is null;
    public ResultErrorCode? Error { get; }
    public string? Message { get; }
    internal Result(T value, ResultErrorCode? errorCode, string? message)
    {
        Value = value;
        Error = errorCode;
        Message = message;
    }
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failed(ResultErrorCode errorCode, string message) => new(default!, errorCode, message);
    public static implicit operator Result(Result<T> result) => new(result.Error, result.Message);
    public static implicit operator Result<T>(Result result) => new(default!, result.Error, result.Message);
}