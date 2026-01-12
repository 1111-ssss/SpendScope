using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Category> _categoryRepository;
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;
    private readonly IFileStorage _fileStorage;
    public DeleteCategoryCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<Category> categoryRepository,
        ILogger<DeleteCategoryCommandHandler> logger,
        IFileStorage fileStorage
    )
    {
        _uow = uow;
        _categoryRepository = categoryRepository;
        _logger = logger;
        _fileStorage = fileStorage;
    }
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);

        if (category == null)
            return Result.NotFound("Категория не найдена");

        var iconPath = _fileStorage.GetFilePath(category.IconUrl);
        if (!string.IsNullOrEmpty(iconPath))
        {
            try
            {
                await _fileStorage.DeleteFileAsync(iconPath, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении иконки категории {request.CategoryId}");
                return Result.InternalServerError("Ошибка при удалении иконки категории");
            }
        }

        await _categoryRepository.DeleteAsync(category, ct);

        try
        {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при удалении категории {request.CategoryId}");
            return Result.InternalServerError("Ошибка при удалении категории");
        }

        return Result.Success();
    }
}