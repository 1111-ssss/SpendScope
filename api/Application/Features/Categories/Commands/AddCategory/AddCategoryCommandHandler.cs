using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Categories.AddCategory;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Result<CategoryResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Category> _categoryRepository;
    private readonly ILogger<AddCategoryCommandHandler> _logger;
    private readonly IImageFormatter _imageFormatter;
    public AddCategoryCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<Category> categoryRepository,
        ILogger<AddCategoryCommandHandler> logger,
        IImageFormatter imageFormatter
    )
    {
        _uow = uow;
        _categoryRepository = categoryRepository;
        _logger = logger;
        _imageFormatter = imageFormatter;
    }
    public async Task<Result<CategoryResponse>> Handle(AddCategoryCommand request, CancellationToken ct)
    {
        var category = Category.Create(request.Name, request.Description);

        if (request.IconFile != null)
        {
            var iconName = $"categories/{Guid.NewGuid()}.png";
            var iconFile = await _imageFormatter.FormatImageAsync(request.IconFile, iconName, ct);

            if (iconFile.IsSuccess)
            {
                category.Update(iconUrl: iconName);
            }
        }
        
        await _categoryRepository.AddAsync(category, ct);

        try
        {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении категории");
            return Result.InternalServerError("Ошибка при добавлении категории");
        }
        
        return Result<CategoryResponse>.Success(new CategoryResponse(
            Id: category.Id,
            Name: category.Name,
            Description: category.Description ?? "",
            IconUrl: category.IconUrl ?? "categories/default.png"
        ));
    }
}