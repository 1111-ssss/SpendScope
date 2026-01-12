using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Category> _categoryRepository;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileStorage _fileStorage;
    private readonly IImageFormatter _imageFormatter;
    public UpdateCategoryCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<Category> categoryRepository,
        ILogger<UpdateCategoryCommandHandler> logger,
        ICurrentUserService currentUserService,
        IFileStorage fileStorage,
        IImageFormatter imageFormatter
    )
    {
        _uow = uow;
        _categoryRepository = categoryRepository;
        _logger = logger;
        _currentUserService = currentUserService;
        _fileStorage = fileStorage;
        _imageFormatter = imageFormatter;
    }
    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);

        if (category == null)
            return Result.NotFound("Категория не найдена");

        category.Update(name: request.Name, description: request.Description);

        var iconPath = category.IconUrl;
        if (request.IconFile != null && category.IconUrl == null)
            iconPath = $"categories/{Guid.NewGuid()}.png";

        await _categoryRepository.UpdateAsync(category, ct);

        try
        {
            await _uow.SaveChangesAsync(ct);

            if (request.IconFile != null)
            {
                var result = await _imageFormatter.FormatImageAsync(request.IconFile, iconPath!, ct);
                
                return result.Bind(() => new CategoryResponse(
                    Category: category
                ));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при обновлении категории {request.CategoryId} пользователем {_currentUserService.GetUserId()}");
            return Result.InternalServerError("Ошибка при обновлении категории");
        }

        return Result<CategoryResponse>.Success(new CategoryResponse(
            Category: category
        ));
    }
}