using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Categories.GetCategories;

public record GetCategoriesQuery() : IRequest<Result<GetCategoriesResponse>>;