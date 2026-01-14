using Domain.Entities;

namespace Application.Features.Categories;

public record CategoryResponse(int Id, string Name, string? Description, string IconUrl);