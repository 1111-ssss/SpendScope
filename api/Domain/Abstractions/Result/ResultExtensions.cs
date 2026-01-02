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
}