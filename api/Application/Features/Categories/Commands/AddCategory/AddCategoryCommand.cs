using Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.AddCategory;

public record AddCategoryCommand(string Name, string? Description = "", IFormFile? IconFile = null) : IRequest<Result<CategoryResponse>>;