using Application.Abstractions.Misc;
using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Stats.GetRequestCount;

public class GetRequestCountHandler : IRequestHandler<GetRequestCountQuery, Result<int>>
{
    private readonly IRequestStatisticsService _requestStatisticsService;
    public GetRequestCountHandler(
        IRequestStatisticsService requestStatisticsService
    )
    {
        _requestStatisticsService = requestStatisticsService;
    }
    public async Task<Result<int>> Handle(GetRequestCountQuery request, CancellationToken ct)
    {
        var count = _requestStatisticsService.GetRequestCountAsync(request.DateTime);

        return Result<int>.Success(
            count
        );
    }
}