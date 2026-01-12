using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Features.Categories.GetCategories;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Categories.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<GetCategoriesResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Category> _categoryRepository;
    private readonly ILogger<GetCategoriesQueryHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    public GetCategoriesQueryHandler(
        IUnitOfWork uow,
        IBaseRepository<Category> categoryRepository,
        ILogger<GetCategoriesQueryHandler> logger,
        ICurrentUserService currentUserService
    )
    {
        _uow = uow;
        _categoryRepository = categoryRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }
    public async Task<Result<GetCategoriesResponse>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var categories = await _categoryRepository.ListAsync(ct);
        
        return Result<GetCategoriesResponse>.Success(new GetCategoriesResponse(
            Categories: categories.ToArray(),
            TotalCount: categories.Count
        ));
    }
}