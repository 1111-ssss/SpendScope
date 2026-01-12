using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Categories.DeleteCategory;

public record DeleteCategoryCommand(int CategoryId) : IRequest<Result>;