using System.Net.NetworkInformation;

namespace Domain.Abstractions.Result;

public interface IResultBase
{
    bool IsSuccess { get; }
    ErrorCode? Error { get; }
    string? Message { get; }
}
public sealed class Result : IResultBase
{
    public bool IsSuccess => Error is null;
    public ErrorCode? Error { get; }
    public string? Message { get; }
    internal Result(ErrorCode? errorCode = null, string? message = null)
    {
        Error = errorCode;
        Message = message;
    }
    public static Result Success() => new();
    public static Result Failed(ErrorCode errorCode, string? message) => new(errorCode, message);

    public static Result Unauthorized(string message = "Пользователь не авторизован") => Failed(ErrorCode.Unauthorized, message);
    public static Result Forbidden(string message = "Доступ запрещен") => Failed(ErrorCode.Forbidden, message);
    public static Result NotFound(string message = "Не найдено") => Failed(ErrorCode.NotFound, message);
    public static Result BadRequest(string message = "Неверный запрос") => Failed(ErrorCode.BadRequest, message);
    public static Result InternalServerError(string message = "Внутренняя ошибка сервера") => Failed(ErrorCode.InternalServerError, message);
    public static Result ValidationFailed(string message = "Ошибка валидации") => Failed(ErrorCode.ValidationFailed, message);
}
public sealed class Result<T> : IResultBase
{
    public Result(T value)
    {
        Value = value;
    }
    public T Value { get; }
    public bool IsSuccess => Error is null;
    public ErrorCode? Error { get; }
    public string? Message { get; }
    internal Result(T value, ErrorCode? error, string? message)
    {
        Value = value;
        Error = error;
        Message = message;
    }
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failed(ErrorCode errorCode, string message) => new(default!, errorCode, message);
    public static implicit operator Result(Result<T> result) => new(result.Error, result.Message);
    public static implicit operator Result<T>(Result result) => new(default!, result.Error, result.Message);
}