using Domain.Abstractions.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Storage;

public interface IImageFormatter
{
    Task<Result> FormatImageAsync(IFormFile file, string savePath, CancellationToken ct = default);
}