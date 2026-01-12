using Domain.Entities;

namespace Application.Features.Categories.GetCategories;

public record GetCategoriesResponse(Category[] Categories, int TotalCount);