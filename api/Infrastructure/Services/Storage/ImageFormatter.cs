using Application.Abstractions.Storage;
using Domain.Abstractions.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Services.Storage
{
    public class ImageFormatter : IImageFormatter
    {
        private readonly ILogger<ImageFormatter> _logger;
        public ImageFormatter(ILogger<ImageFormatter> logger)
        {
            _logger = logger;
        }
        public async Task<Result> FormatImage(IFormFile file, string savePath, CancellationToken ct = default)
        {
            try
            {
                using var image = await Image.LoadAsync(file.OpenReadStream(), ct);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(512, 512),
                    Mode = ResizeMode.Max
                }));

                await image.SaveAsPngAsync(savePath, ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке изображения");
                return Result.Failed(ErrorCode.BadRequest, "Невалидное или повреждённое изображение");
            }
        }
    }
}