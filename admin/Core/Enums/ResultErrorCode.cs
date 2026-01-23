namespace admin.Core.Enums;

public enum ResultErrorCode
{
    Unknown = 0,

    BadRequest = 400,
    ValidationFailed = 422,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    TooManyRequests = 429,
    InternalServerError = 500,
    ServiceUnavailable = 503,

    ReadWrite = 1000,
    BadArguments = 1001,
    DeserializationFailed = 1002,
}
