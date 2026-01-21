using System.Net;

namespace admin.Core.Model;

public interface IResultBase
{
    bool IsSuccess { get; }
    HttpStatusCode? Error { get; }
    string? Message { get; }
}
public sealed class Result : IResultBase
{
    public bool IsSuccess => Error is null;
    public HttpStatusCode? Error { get; }
    public string? Message { get; }
    internal Result(HttpStatusCode? errorCode = null, string? message = null)
    {
        Error = errorCode;
        Message = message;
    }
    public static Result Success() => new();
    public static Result Failed(HttpStatusCode errorCode, string? message) => new(errorCode, message);

    public static Result Unauthorized(string message = "Пользователь не авторизован") => Failed(HttpStatusCode.Unauthorized, message);
    public static Result Forbidden(string message = "Доступ запрещен") => Failed(HttpStatusCode.Forbidden, message);
    public static Result NotFound(string message = "Не найдено") => Failed(HttpStatusCode.NotFound, message);
    public static Result BadRequest(string message = "Неверный запрос") => Failed(HttpStatusCode.BadRequest, message);
    public static Result InternalServerError(string message = "Внутренняя ошибка сервера") => Failed(HttpStatusCode.InternalServerError, message);
    public static Result ValidationFailed(string message = "Ошибка валидации") => Failed(HttpStatusCode.UnprocessableEntity, message);
}
public sealed class Result<T> : IResultBase
{
    public Result(T value)
    {
        Value = value;
    }
    public T Value { get; }
    public bool IsSuccess => Error is null;
    public HttpStatusCode? Error { get; }
    public string? Message { get; }
    internal Result(T value, HttpStatusCode? error, string? message)
    {
        Value = value;
        Error = error;
        Message = message;
    }
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failed(HttpStatusCode errorCode, string message) => new(default!, errorCode, message);
    public static implicit operator Result(Result<T> result) => new(result.Error, result.Message);
    public static implicit operator Result<T>(Result result) => new(default!, result.Error, result.Message);
}