using System.Globalization;

namespace Domain.Abstractions.Result;

public static class ResultExtensions
{
    public static Result<TOut> Bind<TOut>(this Result source, Func<TOut> onSuccess)
    {
        if (source.IsSuccess)
        {
            return Result<TOut>.Success(onSuccess());
        }
        
        return Result<TOut>.Failed(source.Error!.Value, source.Message ?? "Неизвестная ошибка");
    }
    // public static Result<TOut> Map<TIn, TOut>(this Result<TIn> source, Func<TIn, TOut> mapper)
    // {
    //     if (!source.IsSuccess)
    //     {
    //         return source;
    //     }
        
    //     return Result<TOut>.Success(mapper(source.Value));
    // }
}