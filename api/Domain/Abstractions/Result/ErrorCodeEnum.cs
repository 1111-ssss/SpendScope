namespace Domain.Abstractions.Result
{
    public enum ErrorCode
    {
        // special
        None = 0,
        Unknown = 1,

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
        InvalidEmail = 1002,
        UserNotFound = 1003,
        TokenExpired = 1005,
        NotAllowed = 1006,
        
    }
}