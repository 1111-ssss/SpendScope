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
            409 => new ConflictObjectResult(new { error = result.Message ?? "Conflict", code }),
            404 => new NotFoundObjectResult(new { error = result.Message ?? "Not found", code }),
            401 => new UnauthorizedObjectResult(new { error = result.Message ?? "Unauthorized", code }),
            _ => new ObjectResult(new { error = result.Message ?? "Bad request", code }) { StatusCode = code },
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