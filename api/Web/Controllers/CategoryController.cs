using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Web.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Categories.AddCategory;
using Application.Features.Categories.UpdateCategory;
using Application.Features.Categories.DeleteCategory;
using Application.Features.Categories.GetCategories;

namespace Web.Controllers;

[ApiController]
[Route("api/category")]
[Tags("Категории")]
[Authorize]
[ApiVersion("1.0")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Consumes("multipart/form-data")]
    [RequestFormLimits(MultipartBodyLengthLimit = 5_000_000)]
    [RequestSizeLimit(5_000_000)]
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromForm] AddCategoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(); 
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpPatch]
    public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(); 
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    public async Task<IActionResult> DeleteCategory([FromBody] DeleteCategoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(); 
    }
    [HttpGet]
    public async Task<IActionResult> GetCategories(GetCategoriesQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(); 
    }
}