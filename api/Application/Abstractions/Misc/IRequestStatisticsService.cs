namespace Application.Abstractions.Misc;

public interface IRequestStatisticsService
{
    void EnterRequest();
    void ExitRequest();
    int GetActiveConnections();
    void AddFailedRequest();
    int GetFailedRequests();
    int GetTotalRequests();
    void AddRequest(DateTime dateTime);
    int GetRequestCountAsync(DateTime dateTime);
}