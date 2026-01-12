using Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.UpdateCategory;

public record UpdateCategoryCommand(int CategoryId, string? Name = null, string? Description = null, IFormFile? IconFile = null) : IRequest<Result<CategoryResponse>>;