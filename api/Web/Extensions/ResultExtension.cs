using System;
using Microsoft.AspNetCore.Mvc;
using Domain.Abstractions.Result;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this IResultT result)
    {
        if (result == null)
            return new BadRequestObjectResult(new { error = "result is null" });

        if (result.IsSuccess)
            return new OkResult();

        if (!result.Error.HasValue)
            return new BadRequestObjectResult(new { error = result.Message ?? "Bad request" });

        int code = (int)result.Error;

        return code switch
        {
            200 => new ObjectResult(new { message = result.Message ?? "Success" }) { StatusCode = 200 },
            400 => new BadRequestObjectResult(new { error = result.Message ?? "Bad request", code }),
            401 => new UnauthorizedObjectResult(new { error = result.Message ?? "Unauthorized", code }),
            403 => new ObjectResult(new { message = result.Message ?? "Not found" }) { StatusCode = 403 },
            404 => new NotFoundObjectResult(new { error = result.Message ?? "Not found", code }),
            409 => new ConflictObjectResult(new { error = result.Message ?? "Conflict", code }),
            429 => new ObjectResult(new { message = result.Message ?? "Too many requests" }) { StatusCode = 429},
            500 => new ObjectResult(new { message = result.Message ?? "Internal server error" }) { StatusCode = 500},
            503 => new ObjectResult(new { message = result.Message ?? "Service unavailable" }) { StatusCode = 503},
            _ => new ObjectResult(new { error = result.Message ?? "Unknown", code }) { StatusCode = code },
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result == null)
            return new BadRequestObjectResult(new { error = "result is null" });

        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return ((IResultT)result).ToActionResult();
    }
}