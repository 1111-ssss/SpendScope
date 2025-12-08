namespace Domain.Model.Result
{
    /// <summary>
    /// Рекомендуется использовать HTTP-коды для стандартных ошибок и
    /// отдельный диапазон (1000+) для доменных (бизнес) ошибок.
    /// </summary>
    public enum ErrorCode
    {
        // special
        None = 0,
        Unknown = 520, // неизвестная/непредвиденная ошибка

        // HTTP-like common errors
        BadRequest = 400,
        ValidationFailed = 422,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        TooManyRequests = 429,
        InternalServerError = 500,
        ServiceUnavailable = 503,

        // Application / domain specific (>=1000)
        UserAlreadyExists = 1001,
        InvalidCredentials = 1002,
        UserNotActivated = 1003,
        PasswordTooWeak = 1004,
        TokenExpired = 1005
    }
}