namespace Application.Abstractions.Misc;

public interface IRequestStatisticsService
{
    void AddRequest(DateTime dateTime);
    int GetRequestCountAsync(DateTime dateTime);
}